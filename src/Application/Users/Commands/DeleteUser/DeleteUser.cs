using Application.Common.Interfaces;
using Application.Common.Models.Dtos;

namespace Application.Users.Commands;

public record DeleteUserCommand : IRequest<UserDto> {
    public Guid Id { get; init; }
}

public class DeleteUserCommandHandler : IRequestHandler<DeleteUserCommand, UserDto> {
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public DeleteUserCommandHandler(IMapper mapper, IApplicationDbContext context)
    {
        _mapper = mapper;
        _context = context;
    }

    public async Task<UserDto> Handle(DeleteUserCommand request, CancellationToken cancellationToken) {
        var user = await _context.Users.FirstOrDefaultAsync(x => x.Id == request.Id && x.IsActive, cancellationToken);

        if (user is null) {
            throw new KeyNotFoundException($"User with id {request.Id} does not exist");
        }

        user.IsActive = false;
        _context.Users.Update(user);
        await _context.SaveChangesAsync(cancellationToken);

        return _mapper.Map<UserDto>(user);
    }
}
