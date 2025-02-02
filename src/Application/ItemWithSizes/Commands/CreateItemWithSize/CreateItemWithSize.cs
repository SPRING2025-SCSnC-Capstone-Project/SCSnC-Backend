using Application.Common.Interfaces;
using Application.Common.Models.Dtos;
using Domain.Entities;

namespace Application.ItemWithSizes.Commands.CreateItemWithSize;

public record CreateItemWithSizeCommand : IRequest<ItemWithSizeDto>
{
    public Guid ItemId { get; init; }
    public Guid SizeId { get; init; }
}

public class CreateItemWithSizeCommandHandler : IRequestHandler<CreateItemWithSizeCommand, ItemWithSizeDto>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;
    
    public CreateItemWithSizeCommandHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }
    
    public async Task<ItemWithSizeDto> Handle(CreateItemWithSizeCommand request, CancellationToken cancellationToken)
    {
        var item = await _context.Items.FirstOrDefaultAsync(x => x.Id == request.ItemId, cancellationToken);
        var size = await _context.Sizes.FirstOrDefaultAsync(x => x.Id == request.SizeId, cancellationToken);
        
        if (item is null)
        {
            throw new KeyNotFoundException($"Item with id {request.ItemId} not found");
        }
        
        if (size is null)
        {
            throw new KeyNotFoundException($"Size with id {request.SizeId} not found");
        }
        
        var itemWithSize = new ItemWithSize
        {
            Item = item,
            Size = size
        };
        
        var result = _context.ItemWithSizes.Add(itemWithSize);
        
        await _context.SaveChangesAsync(cancellationToken);
        
        return _mapper.Map<ItemWithSizeDto>(result);
    }
}