using Application.Common.Interfaces;
using Application.Common.Models.Dtos;
using NodaTime;

namespace Application.ItemPricesAtBranches.Commands.UpdateItemPriceAtBranch;

public record UpdateItemPriceAtBranchCommand : IRequest<ItemPriceAtBranchDto>
{
    public Guid BranchId { get; init; }
    public Guid ItemId { get; init; }
    public double Price { get; init; }
}

public class UpdateItemPriceAtBranchCommandHandler : IRequestHandler<UpdateItemPriceAtBranchCommand, ItemPriceAtBranchDto>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;
    
    public UpdateItemPriceAtBranchCommandHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }
    
    public async Task<ItemPriceAtBranchDto> Handle(UpdateItemPriceAtBranchCommand request, CancellationToken cancellationToken)
    {
        var itemPriceAtBranch = await _context.ItemPricesAtBranches
            .FirstOrDefaultAsync(x => x.BranchId == request.BranchId && x.ItemId == request.ItemId, cancellationToken);

        if (itemPriceAtBranch is null)
        {
            throw new KeyNotFoundException($"Item price at branch with branch id {request.BranchId} and item id {request.ItemId} not found");
        }
        
        itemPriceAtBranch.Price = request.Price;
        itemPriceAtBranch.LastUpdatedAt = LocalDateTime.FromDateTime(DateTime.Now);
        
        _context.ItemPricesAtBranches.Update(itemPriceAtBranch);
        await _context.SaveChangesAsync(cancellationToken);
        
        var result = await _context.ItemPricesAtBranches
            .Include(x => x.Item)
            .Include(x => x.Branch)
            .FirstOrDefaultAsync(x => x.BranchId == request.BranchId && x.ItemId == request.ItemId, cancellationToken);
        
        return _mapper.Map<ItemPriceAtBranchDto>(result);
    }
}