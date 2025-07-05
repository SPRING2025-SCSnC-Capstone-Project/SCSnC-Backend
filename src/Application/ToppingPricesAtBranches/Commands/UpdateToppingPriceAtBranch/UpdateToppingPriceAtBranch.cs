using Application.Common.Interfaces;
using Application.Common.Models.Dtos;
using NodaTime;

namespace Application.ToppingPricesAtBranches.Commands.UpdateToppingPriceAtBranch;

public record UpdateToppingPriceAtBranchCommand : IRequest<ToppingPriceAtBranchDto>
{
    public Guid ToppingId { get; init; }
    public Guid BranchId { get; init; }
    public double Price { get; init; }
}

public class UpdateToppingPriceAtBranchCommandHandler : IRequestHandler<UpdateToppingPriceAtBranchCommand, ToppingPriceAtBranchDto>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public UpdateToppingPriceAtBranchCommandHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<ToppingPriceAtBranchDto> Handle(UpdateToppingPriceAtBranchCommand request, CancellationToken cancellationToken)
    {
        var toppingPriceAtBranch = await _context.ToppingPricesAtBranches
            .FirstOrDefaultAsync(x => x.ToppingId == request.ToppingId && x.BranchId == request.BranchId, cancellationToken);

        if (toppingPriceAtBranch == null)
        {
            throw new KeyNotFoundException($"Topping price at branch with branch id {request.BranchId} and Topping id {request.ToppingId} not found");
        }

        toppingPriceAtBranch.ToppingPrice = request.Price;
        toppingPriceAtBranch.LastUpdatedAt = LocalDateTime.FromDateTime(DateTime.Now);

        _context.ToppingPricesAtBranches.Update(toppingPriceAtBranch);
        await _context.SaveChangesAsync(cancellationToken);

        var result = await _context.ToppingPricesAtBranches
            .Include(x => x.Branch)
            .Include(x => x.Topping)
            .FirstOrDefaultAsync(x => x.Id == toppingPriceAtBranch.Id && x.ToppingId == request.ToppingId, cancellationToken);
        
        return _mapper.Map<ToppingPriceAtBranchDto>(result);
    }
}