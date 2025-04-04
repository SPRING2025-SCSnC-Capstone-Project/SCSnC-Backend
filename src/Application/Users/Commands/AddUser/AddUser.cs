using Application.Common.Exceptions;
using Application.Common.Helpers;
using Application.Common.Interfaces;
using Application.Common.Models.Dtos;
using Domain.Entities;
using NodaTime;

namespace Application.Users.Commands;

public record AddUserCommand : IRequest<UserDto> {
    public string Username { get; init; } = null!;
    public string? Password { get; init; }
    public string? FullName { get; init; }
    public string Email { get; init; } = null!;
    public string? Phone { get; init; }
    public string? Address { get; init; }
    public string Role { get; init; } = null!;
    public string? AvatarLink { get; init; }
    public string Type { get; init; } = null!;
}

public class AddUserCommandHandler : IRequestHandler<AddUserCommand, UserDto> {
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;
    private readonly ISecurityService _securityService;

    public AddUserCommandHandler(IApplicationDbContext context, IMapper mapper, ISecurityService securityService)
    {
        _context = context;
        _mapper = mapper;
        _securityService = securityService;
    }

    public async Task<UserDto> Handle(AddUserCommand request, CancellationToken cancellationToken) {
        var username_used = await _context.Users.FirstOrDefaultAsync(x => x.Username.Equals(request.Username.Trim()) && x.IsActive, cancellationToken);
        var email_used = await _context.Users.FirstOrDefaultAsync(x => x.Email.Equals(request.Email.Trim()) && x.IsActive, cancellationToken);

        if (string.IsNullOrEmpty(request.Type) && 
                string.IsNullOrEmpty(request.Password)) {
            throw new RequestValidationException("Password is required");
        }

        if (username_used is not null) {
            throw new ConflictException($"User with username {request.Username} already exists"); 
        }

        if (email_used is not null) {
            throw new ConflictException($"This email has already been used");
        }

        var salt = StringUtils.RandomString(15);
        Console.WriteLine(salt);

        var user = new User() {
            Username = request.Username,
            Role = request.Role,
            Email = request.Email,
            Phone = request.Phone,
            Address = request.Address,
            FullName = request.FullName,
            IsActive = true,
            CreatedAt = LocalDateTime.FromDateTime(DateTime.Now),
            LastUpdatedAt = LocalDateTime.FromDateTime(DateTime.Now),
            AvatarLink = request.AvatarLink,
            AccountType = string.IsNullOrEmpty(request.Type) ? "manual" : "google",
        };

        if (string.IsNullOrEmpty(request.Type)) {
            var hashedPassword = _securityService.Hash(request.Password!, salt, request.Email);
            var b64HashedPassword = Convert.ToBase64String(hashedPassword);
            user.PasswordHash = $"{salt}{b64HashedPassword}";
        }

        var res = await _context.Users.AddAsync(user, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);

        return _mapper.Map<UserDto>(res.Entity);
    }
}
