using Application.Common.Interfaces;
using Application.Common.Models.Dtos;

namespace Application.Users.Commands;

public record UpdateUserCommand: IRequest<UserDto> {
    public Guid Id { get; init; }
    public string? Username { get; init; }
    public string? FullName { get; init; }
    public string? Phone { get; init; }
    public string? Address { get; init; }
    public string? AvatarLink { get; init; }
}

public class UpdateUserCommandHandler: IRequestHandler<UpdateUserCommand, UserDto> {
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public UpdateUserCommandHandler(IMapper mapper, IApplicationDbContext context)
    {
        _mapper = mapper;
        _context = context;
    }

    public async Task<UserDto> Handle (UpdateUserCommand request, CancellationToken cancellationToken) {
        var user = await _context.Users.FirstOrDefaultAsync(x => x.Id == request.Id && x.IsActive, cancellationToken);

        if (user is null) {
            throw new KeyNotFoundException($"User with id {request.Id} does not exist");
        }

        user.Username = request.Username ?? user.Username;
        user.FullName = request.FullName ?? user.FullName;
        user.Phone = request.Phone ?? user.Phone;
        user.Address = request.Address ?? user.Address;
        user.AvatarLink = request.AvatarLink ?? user.AvatarLink;

        _context.Users.Update(user);
        await _context.SaveChangesAsync(cancellationToken);

        return _mapper.Map<UserDto>(user);
    }
}
