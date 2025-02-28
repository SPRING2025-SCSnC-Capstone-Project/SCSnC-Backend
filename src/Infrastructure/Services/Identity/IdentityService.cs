using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Application.Common.Models.Dtos;
using AutoMapper;
using Domain.Entities;
using Microsoft.AspNetCore.Authentication;
using Microsoft.EntityFrameworkCore;
using OneOf;

namespace Infrastructure.Services.Identity;

public class IdentityService : IIdentityService {
    private readonly IApplicationDbContext _context;
    private readonly ISecurityService _securityService;
    private readonly IMapper _mapper;

    public IdentityService(IApplicationDbContext context, ISecurityService securityService, IMapper mapper)
    {
        _context = context;
        _securityService = securityService;
        _mapper = mapper;
    }

    public async Task<OneOf<(UserDto, ClaimsIdentity), string>> AuthenticateAsync(string email, string password, CancellationToken cancellationToken) {
        var user = await _context.Users.FirstOrDefaultAsync(x => x.Email.Equals(email) 
                && x.IsActive && x.AccountType.Equals("manual"), cancellationToken: cancellationToken);

        if (user is null || string.IsNullOrEmpty(password)) {
            throw new AuthenticationFailureException("Invalid email or password");
        }

        var base64Salt = user.PasswordHash![..20];
        var hashedInputPassword = _securityService.Hash(password, base64Salt, user.Username);

        var base64HashedPassword = user.PasswordHash[20..];
        var hashedPassword = Convert.FromBase64String(base64HashedPassword);

        if (!hashedInputPassword.SequenceEqual(hashedPassword)) {
            throw new AuthenticationFailureException("Invalid email or password");
        }

        var claims = CreateClaims(user);
        return (_mapper.Map<UserDto>(user), claims);
    }

    private ClaimsIdentity CreateClaims(User user)
    {
        var claimsIdentity = new ClaimsIdentity();

        claimsIdentity.AddClaims([
            new(JwtRegisteredClaimNames.NameId, user.Id.ToString()),
            new(JwtRegisteredClaimNames.Email, user.Email),
            new(JwtRegisteredClaimNames.Sub, user.Username),
        ]);

        if (!string.IsNullOrEmpty(user.FullName))
        {
            claimsIdentity.AddClaim(new(JwtRegisteredClaimNames.Name, user.FullName));
        }

        return claimsIdentity;
    }

}
