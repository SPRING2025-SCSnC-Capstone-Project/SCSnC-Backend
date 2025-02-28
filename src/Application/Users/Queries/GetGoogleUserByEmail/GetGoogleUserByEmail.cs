using Application.Common.Interfaces;
using Application.Common.Models.Dtos;

namespace Application.Users.Queries;

public record GetGoogleUserByEmailQuery : IRequest<UserDto?> {
    public string Email { get; init; } = null!;
}

public class GetGoogleUserByEmailQueryHandler : IRequestHandler<GetGoogleUserByEmailQuery, UserDto?> {
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetGoogleUserByEmailQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<UserDto?> Handle(GetGoogleUserByEmailQuery request, CancellationToken cancellationToken) {
        var user = await _context.Users
            .FirstOrDefaultAsync(x => x.Email.Equals(request.Email.Trim()) 
                    && x.IsActive && x.AccountType.Equals("google"), cancellationToken);

        if (user is null) {
            return null;
        }

        return _mapper.Map<UserDto>(user);
    }
} 
