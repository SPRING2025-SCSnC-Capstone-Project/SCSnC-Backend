using Application.Common.Interfaces;
using Application.Common.Models.Dtos;

namespace Application.Reservations.Queries.GetReservationById;

public record GetReservationByIdQuery: IRequest<ReservationDto>
{
    public Guid Id { get; set; }
}

public class GetReservationByIdQueryHandler: IRequestHandler<GetReservationByIdQuery, ReservationDto>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;
    
    public GetReservationByIdQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }
    
    public async Task<ReservationDto> Handle(GetReservationByIdQuery request, CancellationToken cancellationToken)
    {
        var entity = _context.Reservations
            .Include (x => x.Workspace)
            .ThenInclude (y => y.WorkspaceTypeAtBranch.WorkspaceType)
            .Include(x => x.User)
            .Include(x => x.ReservedSlots)
            .ThenInclude(y => y.Slot)
            .FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);
        
        if (entity is null)
        {
            throw new KeyNotFoundException($"Reservation with id {request.Id} not found");
        }
        
        var result = _mapper.Map<ReservationDto>(await entity);

        return result;
    }
}
