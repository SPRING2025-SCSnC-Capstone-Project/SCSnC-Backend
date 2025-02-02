using Application.Common.Interfaces;
using Application.Common.Models.Dtos;
using NodaTime;

namespace Application.Items.Commands.UpdateItem;

public record UpdateItemCommand : IRequest<ItemDto>
{
    public Guid Id { get; init; }
    public string Name { get; init; }
    public string Description { get; init; }
    public double Price { get; init; }
    public string Img { get; init; }
    public Guid CategoryId { get; init; }
}

public class UpdateItemCommandHandler : IRequestHandler<UpdateItemCommand, ItemDto>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;
    
    public UpdateItemCommandHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }
    
    public async Task<ItemDto> Handle(UpdateItemCommand request, CancellationToken cancellationToken)
    {
        var item = await _context.Items.FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);
        
        if (item is null)
        {
            throw new KeyNotFoundException($"Item with id {request.Id} not found");
        }
        
        item.ItemName = request.Name;
        item.ItemDescription = request.Description;
        item.ItemBasePrice = request.Price;
        item.ItemCategoryId = request.CategoryId;
        item.ItemImg = request.Img;
        item.LastUpdatedAt = LocalDateTime.FromDateTime(DateTime.Now);
        
        _context.Items.Update(item);
        await _context.SaveChangesAsync(cancellationToken);
        
        return _mapper.Map<ItemDto>(item);
    }
}