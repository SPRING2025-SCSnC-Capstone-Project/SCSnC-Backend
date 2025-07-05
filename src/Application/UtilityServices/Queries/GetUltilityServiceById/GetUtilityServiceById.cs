using Application.Common.Interfaces;
using Application.Common.Models.Dtos;

namespace Application.UtilityServices.Queries.GetUltilityServiceById;

public record GetUtilityServiceByIdQuery : IRequest<UtilityServiceDto>
{
    public Guid Id { get; init; }
}

public class GetUtilityServiceByIdQueryHandler : IRequestHandler<GetUtilityServiceByIdQuery, UtilityServiceDto>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;
    
    public GetUtilityServiceByIdQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }
    
    public async Task<UtilityServiceDto> Handle(GetUtilityServiceByIdQuery request, CancellationToken cancellationToken)
    {
        var entity = await _context.UtilityServices
            .FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);
        
        if (entity is null)
        {
            throw new KeyNotFoundException($"Utility service with Id {request.Id} does not exist");
        }
        
        return _mapper.Map<UtilityServiceDto>(entity);
    }
}