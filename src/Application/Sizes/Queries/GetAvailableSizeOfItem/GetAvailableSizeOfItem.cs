using Application.Common.Interfaces;
using Application.Common.Models.Dtos;

namespace Application.Sizes.Queries.GetAvailableSizeOfItem;

public record GetAvailableSizeOfItemQuery : IRequest<List<SizeDto>>
{
    public Guid ItemId { get; init; }
}

public class GetAvailableSizeOfItemQueryHandler : IRequestHandler<GetAvailableSizeOfItemQuery, List<SizeDto>>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;
    
    public GetAvailableSizeOfItemQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }


    public async Task<List<SizeDto>> Handle(GetAvailableSizeOfItemQuery request, CancellationToken cancellationToken)
    {
        var itemWithSizes = await _context.ItemWithSizes.Where(x => x.ItemId == request.ItemId).ToListAsync(cancellationToken);
        
        var sizes = new List<SizeDto>();
        
        foreach (var itemWithSize in itemWithSizes)
        {
            var size = await _context.Sizes.FirstOrDefaultAsync(x => x.Id == itemWithSize.SizeId, cancellationToken);
            
            sizes.Add(_mapper.Map<SizeDto>(size));
        }
        
        return sizes;
    }
}