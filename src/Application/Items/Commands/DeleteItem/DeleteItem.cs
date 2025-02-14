using Application.Common.Interfaces;
using Application.Common.Models.Dtos;

namespace Application.Items.Commands.DeleteItem;

public record DeleteItemCommand : IRequest<ItemDto>
{
    public Guid Id { get; init; }
}

public class DeleteItemCommandHandler : IRequestHandler<DeleteItemCommand, ItemDto>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;
    
    public DeleteItemCommandHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }
    
    public async Task<ItemDto> Handle(DeleteItemCommand request, CancellationToken cancellationToken)
    {
        var item = await _context.Items.FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);
        
        if (item is null)
        {
            throw new KeyNotFoundException($"Item with id {request.Id} not found");
        }

        item.IsActive = false;
        
        _context.Items.Update(item);
        
        await _context.SaveChangesAsync(cancellationToken);
        
        return _mapper.Map<ItemDto>(item);
    }
}