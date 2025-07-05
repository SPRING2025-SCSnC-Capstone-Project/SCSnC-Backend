using Application.Common.Interfaces;
using Application.Common.Models.Dtos;

namespace Application.ItemCategories.Queries.GetItemCategoryById;

public record GetItemCategoryByIdQuery : IRequest<SingleItemCategoryDto>
{
    public Guid Id { get; init; }
}

public class GetItemCategoryByIdQueryHandler : IRequestHandler<GetItemCategoryByIdQuery, SingleItemCategoryDto>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;
    
    public GetItemCategoryByIdQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }
    
    public async Task<SingleItemCategoryDto> Handle(GetItemCategoryByIdQuery request, CancellationToken cancellationToken)
    {
        var entity = await _context.ItemCategories.Where(c => c.IsActive == true).FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);
        
        if (entity is null)
        {
            throw new KeyNotFoundException($"Item Category with id {request.Id} not found");
        }

        var response = _mapper.Map<SingleItemCategoryDto>(entity);
        
        response.Items = _context.Items
            .Where(i => i.ItemCategoryId == entity.Id && i.IsActive == true)
            .Select(x => _mapper.Map<ItemDto>(x))
            .ToList();
        
        return response;
    }
}