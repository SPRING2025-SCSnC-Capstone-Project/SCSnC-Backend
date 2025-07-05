using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Infrastructure.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure.Identity.Authorization;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
public class RequiresRoleAttribute : Attribute, IAuthorizationFilter
{
    private readonly string[] _claimValues;

    public RequiresRoleAttribute(params string[] claimValues)
    {
        _claimValues = claimValues;
    }

    public void OnAuthorization(AuthorizationFilterContext context)
    {
        var dbContext = context.HttpContext.RequestServices.GetRequiredService<ApplicationDbContext>();

        var email = ((ClaimsIdentity)context.HttpContext.User.Identity!).Claims.SingleOrDefault(y => y.Type.Equals("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/emailaddress"))!.Value;

        var user = dbContext.Users.FirstOrDefault(x => x.Email!.Equals(email));

        if (user is null)
        {
            context.Result = new ForbidResult();
            return;
        }
        
        if (_claimValues.Any(claimValue => user!.Role.Equals(claimValue)))
        {
            return;
        }

        context.Result = new ForbidResult();
    }
}