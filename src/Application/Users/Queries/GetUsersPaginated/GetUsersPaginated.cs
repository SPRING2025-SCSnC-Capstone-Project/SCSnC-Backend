using Application.Common.Extensions;
using Application.Common.Interfaces;
using Application.Common.Models;
using Application.Common.Models.Dtos;
using Domain.Entities;

namespace Application.Users.Queries;

public record GetUsersPaginatedQuery: IRequest<PaginatedList<UserDto>> {
    public int? Page { get; init; }
    public int? Size { get; init; }
    public string? SortBy { get; init; }
    public string? SortOrder { get; init; }
    public string? SearchTerm { get; init; }
}

public class GetUsersPaginatedQueryHandler: IRequestHandler<GetUsersPaginatedQuery, PaginatedList<UserDto>> {
    private IApplicationDbContext _context;
    private IMapper _mapper;

    public GetUsersPaginatedQueryHandler(IApplicationDbContext context, IMapper mapper) {
        _context = context;
        _mapper = mapper;
    }

    public async Task<PaginatedList<UserDto>> Handle(GetUsersPaginatedQuery request, CancellationToken cancellationToken) {
        var users = _context.Users.AsQueryable().Where(x => x.IsActive);

        if (!string.IsNullOrEmpty(request.SearchTerm)) {
            users = users.Where(x => x.Username.Contains(request.SearchTerm));
        } 

        return await users.ListPaginateWithSortAsync<User, UserDto>(
            request.Page,
            request.Size,
            request.SortBy,
            request.SortOrder,
            _mapper.ConfigurationProvider,
            cancellationToken
        );
    }
}
