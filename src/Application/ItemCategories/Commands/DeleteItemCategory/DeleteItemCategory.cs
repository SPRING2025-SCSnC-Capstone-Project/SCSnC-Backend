using Application.Common.Interfaces;
using Application.Common.Models.Dtos;

namespace Application.ItemCategories.Commands.DeleteItemCategory;

public record DeleteItemCategoryCommand : IRequest<ItemCategoryDto>
{
    public Guid Id { get; set; }
}

public class DeleteItemCategoryCommandHandler : IRequestHandler<DeleteItemCategoryCommand, ItemCategoryDto>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;
    
    public DeleteItemCategoryCommandHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }
    
    public async Task<ItemCategoryDto> Handle(DeleteItemCategoryCommand request, CancellationToken cancellationToken)
    {
        var itemCategory = await _context.ItemCategories.FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);
        
        if (itemCategory is null)
        {
            throw new KeyNotFoundException($"Item Category with id {request.Id} not found");
        }
        
        itemCategory.IsActive = false;
        await _context.SaveChangesAsync(cancellationToken);
        
        return _mapper.Map<ItemCategoryDto>(itemCategory);
    }
}