using Application.Common.Interfaces;
using Application.Common.Models.Dtos;

namespace Application.Items.Queries.GetItemById;

public record GetItemByIdQuery : IRequest<ItemInfoDto>
{
    public Guid Id { get; set; }
}

public class GetItemByIdQueryHandler : IRequestHandler<GetItemByIdQuery, ItemInfoDto>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;
    
    public GetItemByIdQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }
    
    public async Task<ItemInfoDto> Handle(GetItemByIdQuery request, CancellationToken cancellationToken)
    {
        var item = await _context.Items
            .Include(x => x.ItemCategory)
            .Include(x => x.ItemWithSizes)
            .ThenInclude(y => y.Size)
            .Include(x => x.ItemPricesAtBranches.Where(y => y.ItemId == request.Id))
            .ThenInclude(br => br.Branch)
            .FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);

        if (item is null)
        {
            throw new KeyNotFoundException($"Item with id {request.Id} not found");
        }
        
        return _mapper.Map<ItemInfoDto>(item);
    }
}