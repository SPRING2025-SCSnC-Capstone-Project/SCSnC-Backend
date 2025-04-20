using Application.Common.Interfaces;
using Application.Common.Models.Dtos;
using System.Diagnostics;

namespace Application.Items.Queries.GetActiveItemById;

public record GetActiveItemByIdQuery : IRequest<ItemDto>
{
    public Guid Id { get; set; }
    public Guid BranchId { get; set; }
}

public class GetActiveItemByIdQueryHandler : IRequestHandler<GetActiveItemByIdQuery, ItemDto>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;
    
    public GetActiveItemByIdQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }
    
    public async Task<ItemDto> Handle(GetActiveItemByIdQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var item = await _context.Items
                .Include(x => x.ItemCategory)
                .Include(x => x.ItemWithSizes)
                .ThenInclude(x => x.Size)
                .Include(x => x.ItemPricesAtBranches.Where(y => y.BranchId == request.BranchId))
                .FirstOrDefaultAsync(x => x.Id == request.Id && x.IsActive == true, cancellationToken);

            if (item is null)
            {
                throw new KeyNotFoundException($"Item with id {request.Id} not found or not active");
            }

            var result = _mapper.Map<ItemDto>(item);

            return result;
        }
        catch (Exception ex)
        {
            Debug.WriteLine(ex);
            throw new Exception(ex.Message);
        }
    }
}