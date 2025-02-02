using Application.Common.Interfaces;
using Application.Common.Models.Dtos;

namespace Application.ItemWithSizes.Commands.DeleteItemWithSize;

public record DeleteItemWithSizeCommand : IRequest<ItemWithSizeDto>
{
    public Guid Id { get; init; }
}

public class DeleteItemWithSizeCommandHandler : IRequestHandler<DeleteItemWithSizeCommand, ItemWithSizeDto>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;
    
    public DeleteItemWithSizeCommandHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }
    
    public async Task<ItemWithSizeDto> Handle(DeleteItemWithSizeCommand request, CancellationToken cancellationToken)
    {
        var itemWithSize = await _context.ItemWithSizes.FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);
        
        if (itemWithSize is null)
        {
            throw new KeyNotFoundException($"Item with size id {request.Id} not found");
        }
        
        _context.ItemWithSizes.Remove(itemWithSize);
        
        await _context.SaveChangesAsync(cancellationToken);
        
        return _mapper.Map<ItemWithSizeDto>(itemWithSize);
    }
}