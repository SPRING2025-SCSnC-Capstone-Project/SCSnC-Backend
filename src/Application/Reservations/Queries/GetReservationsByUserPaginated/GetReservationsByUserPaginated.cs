using Application.Common.Extensions;
using Application.Common.Interfaces;
using Application.Common.Models;
using Application.Common.Models.Dtos;
using Domain.Entities;

namespace Application.Reservations.Queries.GetReservationsByUserPaginated;

public record GetReservationsByUserPaginatedQuery : IRequest<PaginatedList<ReservationDto>>
{
    public Guid UserId { get; init; }
    public int? Page { get; init; }
    public int? Size { get; init; }
    public string? SortBy { get; init; }
    public string? SortOrder { get; init; }
    public string? Filter { get; init; }
}

public class GetReservationsByUserPaginatedQueryHandler : IRequestHandler<GetReservationsByUserPaginatedQuery, PaginatedList<ReservationDto>>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;
    
    public GetReservationsByUserPaginatedQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }
    
    public async Task<PaginatedList<ReservationDto>> Handle(GetReservationsByUserPaginatedQuery request, CancellationToken cancellationToken)
    {
        var query = _context.Reservations.Where(x => x.UserId == request.UserId).AsQueryable();
        
        return await query.ListPaginateWithSortAsync<Reservation, ReservationDto>(
            request.Page, 
            request.Size, 
            request.SortBy, 
            request.SortOrder, 
            _mapper.ConfigurationProvider, 
            cancellationToken
        );
    }
}
