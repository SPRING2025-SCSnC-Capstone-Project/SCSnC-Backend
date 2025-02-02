using Application.Common.Interfaces;
using Application.Common.Models.Dtos;

namespace Application.ItemWithSizes.Queries.GetItemWithSizeById;

public record GetItemWithSizeByIdQuery : IRequest<ItemWithSizeDto>
{
    public Guid Id { get; init; }
}

public class GetItemWithSizeByIdQueryHandler : IRequestHandler<GetItemWithSizeByIdQuery, ItemWithSizeDto>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;
    
    public GetItemWithSizeByIdQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }
    
    public async Task<ItemWithSizeDto> Handle(GetItemWithSizeByIdQuery request, CancellationToken cancellationToken)
    {
        var itemWithSize = await _context.ItemWithSizes
            .Include(x => x.Item)
            .Include(x => x.Size)
            .FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);
        
        if (itemWithSize is null)
        {
            throw new KeyNotFoundException($"Item with size id {request.Id} not found");
        }
        
        return _mapper.Map<ItemWithSizeDto>(itemWithSize);
    }
}