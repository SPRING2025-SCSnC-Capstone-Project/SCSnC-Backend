using Application.Common.Interfaces;
using Application.Common.Models.Dtos;

namespace Application.Users.Queries;

public record GetUserByIdQuery : IRequest<UserDto> {
    public Guid Id { get; init; }
}

public class GetUserByIdQueryHandler : IRequestHandler<GetUserByIdQuery, UserDto> {
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetUserByIdQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<UserDto> Handle(GetUserByIdQuery request, CancellationToken cancellationToken) {
        var user = await _context.Users
            .FirstOrDefaultAsync(x => x.Id.Equals(request.Id) 
                    && x.IsActive, cancellationToken);

        if (user is null) {
            throw new KeyNotFoundException($"User with id {request.Id} does not exist");
        }

        return _mapper.Map<UserDto>(user);
    }
} 
