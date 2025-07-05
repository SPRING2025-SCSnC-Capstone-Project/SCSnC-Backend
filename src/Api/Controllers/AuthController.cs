using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Api.Controllers.Payload.Requests;
using Api.Controllers.Payload.Response;
using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Application.Common.Models;
using Application.Common.Models.Dtos;
using Application.Users.Commands;
using Application.Users.Queries;
using AutoMapper;
using Domain.Entities;
using Google.Apis.Auth;
using MediatR;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[ApiController]
[Route("/api/v1/[controller]/[action]")]
public class AuthController : ControllerBase
{
    private ISender? _mediator;
    protected ISender Mediator => _mediator ??= HttpContext.RequestServices.GetRequiredService<ISender>();
    private readonly IIdentityService _identityService;
    private readonly IJwtSService _jwtService;
    private readonly IMapper _mapper;

    public AuthController(IIdentityService identityService, IJwtSService jwtService, IMapper mapper)
    {
        _identityService = identityService;
        _jwtService = jwtService;
        _mapper = mapper;
    }

    [HttpPost]
    public async Task<ActionResult<Result<LoginSuccessResponse>>> VerifyGoogleToken(
            [FromBody] string Token, CancellationToken cancellationToken)
    {
        try
        {
            var res = await GoogleJsonWebSignature.ValidateAsync(Token);

            var googleUserQuery = new GetGoogleUserByEmailQuery()
            {
                Email = res.Email,
            };

            var existingGoogleUser = await Mediator.Send(googleUserQuery, cancellationToken);

            var handler = new JwtSecurityTokenHandler();

            if (existingGoogleUser != null)
            {
                var (gJwtToken, gRefreshToken) = _jwtService
                    .SignInAsync(existingGoogleUser, cancellationToken).Result;

                var gAccessToken = handler.WriteToken(gJwtToken);

                SetJwtAccessToken(gAccessToken, gJwtToken);
                SetRefreshToken(gRefreshToken);

                var gLoginResponse = new LoginSuccessResponse()
                {
                    User = existingGoogleUser,
                    AccessToken = gAccessToken,
                    RefreshToken = gRefreshToken.Token,
                };

                return Ok(Result<LoginSuccessResponse>.Succeed(gLoginResponse));
            }

            var manualUserQuery = new GetManualUserByEmailQuery()
            {
                Email = res.Email,
            };

            var existingManualUser = await Mediator.Send(manualUserQuery, cancellationToken);

            if (existingManualUser != null)
            {
                throw new ConflictException($"User with email {res.Email} has already been created manually. Please login manually to continue");
            }

            var command = new AddUserCommand()
            {
                Email = res.Email,
                Username = res.Email.Split('@')[0],
                Role = "user",
                Type = "google",
                AvatarLink = res.Picture,
                FullName = res.Name,
            };

            var newUser = await Mediator.Send(command, cancellationToken);

            var (jwtToken, refreshToken) = _jwtService
                .SignInAsync(newUser, cancellationToken).Result;

            var accessToken = handler.WriteToken(jwtToken);

            SetJwtAccessToken(accessToken, jwtToken);
            SetRefreshToken(refreshToken);

            var loginResponse = new LoginSuccessResponse()
            {
                User = newUser,
                AccessToken = accessToken,
                RefreshToken = refreshToken.Token,
            };

            return Ok(Result<LoginSuccessResponse>.Succeed(loginResponse));
        }
        catch (InvalidJwtException)
        {
            throw new AuthenticationFailureException("Invalid Token");
        }
    }
    [HttpPost]
    public async Task<IActionResult> Login([FromBody] LoginRequest request, CancellationToken cancellationToken)
    {
        var result = await _identityService.AuthenticateAsync(request.Email, request.Password, cancellationToken);

        return result.Match<IActionResult>((loginSuccess) =>
        {
            var user = loginSuccess;
            var (jwtToken, refreshToken) = _jwtService.SignInAsync(user, cancellationToken).Result;

            var handler = new JwtSecurityTokenHandler();
            var accessToken = handler.WriteToken(jwtToken);

            SetJwtAccessToken(accessToken, jwtToken);
            SetRefreshToken(refreshToken);

            var loginResult = new LoginSuccessResponse()
            {
                User = _mapper.Map<UserDto>(user),
                AccessToken = accessToken,
                RefreshToken = refreshToken.Token,
            };

            return Ok(Result<LoginSuccessResponse>.Succeed(loginResult));
        },
        token =>
        {
            throw new NotImplementedException();
        });
    }

    [HttpPost]
    [Authorize]
    public async Task<IActionResult> Logout()
    {
        var refreshToken = Request.Cookies[nameof(RefreshToken)];
        var jwtToken = Request.Cookies["SCSnCJwtToken"];
        
        Response.Cookies.Delete("SCSnCJwtToken");
        Response.Cookies.Delete(nameof(RefreshToken));
        
        await _jwtService.LogoutAsync(jwtToken!, refreshToken!);

        return Ok();
    }

    [AllowAnonymous]
    [HttpPost]
    public async Task<IActionResult> Refresh([FromBody] string? token, CancellationToken cancellationToken)
    {
        var refreshToken = token ?? Request.Cookies["RefreshToken"];
        if (refreshToken == null)
            return BadRequest("Missing refresh token.");

        var (jwtToken, newRefreshToken) = await _jwtService.RefreshTokenAsync(refreshToken, cancellationToken);

        var accessToken = new JwtSecurityTokenHandler().WriteToken(jwtToken);

        SetJwtAccessToken(accessToken, jwtToken);
        SetRefreshToken(newRefreshToken);

        var result = new RefreshTokenResponse
        {
            AccessToken = accessToken,
            RefreshToken = newRefreshToken.Token,
        };
        return Ok(Result<RefreshTokenResponse>.Succeed(result));
    }

    [Authorize]
    [HttpPost]
    public ActionResult Validate()
    {
        return Ok();
    }

    private void SetJwtAccessToken(string accessToken, JwtSecurityToken jwtToken)
    {
        var cookieOptions = new CookieOptions
        {
            HttpOnly = true,
            Expires = jwtToken.ValidTo,
        };

        Response.Cookies.Append("SCSnCJwtToken", accessToken, cookieOptions);
    }

    private void SetRefreshToken(RefreshToken newRefreshToken)
    {
        var cookieOptions = new CookieOptions
        {
            HttpOnly = true,
            Expires = newRefreshToken.ExpiryDateTime.ToDateTimeUnspecified(),
        };
        Response.Cookies.Append(nameof(RefreshToken), newRefreshToken.Token, cookieOptions);
    }
}
