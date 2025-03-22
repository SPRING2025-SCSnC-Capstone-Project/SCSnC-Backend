using Application.Common.Interfaces;
using Application.Common.Models.Dtos;
using Domain.Entities;
using NodaTime;

namespace Application.ItemCategories.Commands.CreateItemCategory;

public record CreateItemCategoryCommand: IRequest<ItemCategoryDto>
{
    public string Name { get; init; }
}

public class CreateItemCategoryCommandHandler : IRequestHandler<CreateItemCategoryCommand, ItemCategoryDto>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;
    
    public CreateItemCategoryCommandHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }
    
    public async Task<ItemCategoryDto> Handle(CreateItemCategoryCommand request, CancellationToken cancellationToken)
    {
        var itemCategory = new ItemCategory
        {
            CategoryName = request.Name,
            IsActive = true,
            CreatedAt = LocalDateTime.FromDateTime(DateTime.Now),
            LastUpdatedAt = LocalDateTime.FromDateTime(DateTime.Now)
        };
        
        await _context.ItemCategories.AddAsync(itemCategory, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
        
        return _mapper.Map<ItemCategoryDto>(itemCategory);
    }
}