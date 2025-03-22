using Application.Common.Interfaces;
using Application.Common.Models.Dtos;
using NodaTime;

namespace Application.ItemCategories.Commands.UpdateItemCategory;

public record UpdateItemCategoryCommand: IRequest<ItemCategoryDto>
{
    public Guid Id { get; init; }
    public string Name { get; init; }
    public bool? IsActive { get; init; }
}

public class UpdateItemCategoryCommandHandler: IRequestHandler<UpdateItemCategoryCommand, ItemCategoryDto>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;
    
    public UpdateItemCategoryCommandHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }
    
    public async Task<ItemCategoryDto> Handle(UpdateItemCategoryCommand request, CancellationToken cancellationToken)
    {
        var itemCategory = await _context.ItemCategories.FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);
        
        if (itemCategory is null)
        {
            throw new KeyNotFoundException($"Item Category with id {request.Id} not found");
        }
        
        itemCategory.CategoryName = request.Name;
        itemCategory.IsActive = request.IsActive.HasValue ? request.IsActive.Value : itemCategory.IsActive;
        itemCategory.LastUpdatedAt = LocalDateTime.FromDateTime(DateTime.Now);
        
        await _context.SaveChangesAsync(cancellationToken);
        
        return _mapper.Map<ItemCategoryDto>(itemCategory);
    }
}