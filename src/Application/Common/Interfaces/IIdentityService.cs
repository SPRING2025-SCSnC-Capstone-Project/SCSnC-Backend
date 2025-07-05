using System.Security.Claims;
using Application.Common.Models.Dtos;
using OneOf;

namespace Application.Common.Interfaces;

public interface IIdentityService {
   public Task<OneOf<UserDto, string>> AuthenticateAsync(string email, string password, CancellationToken cancellationToken);
}
