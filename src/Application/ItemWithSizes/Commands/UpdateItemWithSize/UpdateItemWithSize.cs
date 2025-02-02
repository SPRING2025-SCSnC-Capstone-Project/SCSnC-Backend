using Application.Common.Interfaces;
using Application.Common.Models.Dtos;

namespace Application.ItemWithSizes.Commands.UpdateItemWithSize;

public record UpdateItemWithSizeCommand : IRequest<ItemWithSizeDto>
{
    public Guid Id { get; init; }
    public Guid ItemId { get; init; }
    public Guid SizeId { get; init; }
}

public class UpdateItemWithSizeCommandHandler : IRequestHandler<UpdateItemWithSizeCommand, ItemWithSizeDto>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;
    
    public UpdateItemWithSizeCommandHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }
    
    public async Task<ItemWithSizeDto> Handle(UpdateItemWithSizeCommand request, CancellationToken cancellationToken)
    {
        var itemWithSize = await _context.ItemWithSizes.FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);
        
        if (itemWithSize is null)
        {
            throw new KeyNotFoundException($"Item with size id {request.Id} not found");
        }
        
        itemWithSize.ItemId = request.ItemId;
        itemWithSize.SizeId = request.SizeId;
        
        _context.ItemWithSizes.Update(itemWithSize);
        await _context.SaveChangesAsync(cancellationToken);
        
        return _mapper.Map<ItemWithSizeDto>(itemWithSize);
    }
}