using Application.Common.Interfaces;
using Application.Common.Models.Dtos;
using Application.ItemWithSizes.Commands.CreateItemWithSize;
using Domain.Entities;
using NodaTime;

namespace Application.Items.Commands.AddItem;

public class AddItemCommand : IRequest<ItemDto>
{
    public string Name { get; set; }
    public string Description { get; set; }
    public double Price { get; set; }
    public string Img { get; set; }
    public Guid CategoryId { get; set; }
    public List<Guid> SizeIds { get; set; }
}

public class AddItemCommandHandler : IRequestHandler<AddItemCommand, ItemDto>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public AddItemCommandHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<ItemDto> Handle(AddItemCommand request, CancellationToken cancellationToken)
    {
        var entity = new Item
        {
            ItemName = request.Name,
            ItemDescription = request.Description,
            ItemBasePrice = request.Price,
            ItemCategoryId = request.CategoryId,
            ItemImg = request.Img,
            CreatedAt = LocalDateTime.FromDateTime(DateTime.Now),
            LastUpdatedAt = LocalDateTime.FromDateTime(DateTime.Now)
        };

        var result = await _context.Items.AddAsync(entity, cancellationToken);
        
        await _context.SaveChangesAsync(cancellationToken);
        
//      Note: add to ItemWithSize table when adding new item
        foreach (var size in request.SizeIds)
        {
            var itemWithSize = new ItemWithSize
            {
                ItemId = result.Entity.Id,
                SizeId = size
            };
            
            await _context.ItemWithSizes.AddAsync(itemWithSize, cancellationToken);
        } 
//      Note: end

        return _mapper.Map<ItemDto>(result.Entity);
    }
}