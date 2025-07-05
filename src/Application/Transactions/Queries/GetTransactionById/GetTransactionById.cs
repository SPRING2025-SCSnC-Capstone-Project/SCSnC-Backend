using Application.Common.Interfaces;
using Application.Common.Models.Dtos;

namespace Application.Transactions.Queries.GetTransactionById;

public record GetTransactionByIdQuery: IRequest<DetailTransactionDto>
{
    public Guid Id { get; init; }
}

public class GetTransactionByIdQueryHandler : IRequestHandler<GetTransactionByIdQuery, DetailTransactionDto>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;
    
    public GetTransactionByIdQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }
    
    public async Task<DetailTransactionDto> Handle(GetTransactionByIdQuery request, CancellationToken cancellationToken)
    {
        var entity = await _context.Transactions
            .Include(x => x.Order)
            .Include(x => x.Order.Voucher)
            .Include(x => x.Order.OrderDetails)
            .Include(x => x.Order.Table)
            .FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);
        
        if (entity == null)
        {
            throw new KeyNotFoundException($"Transaction with id {request.Id} not found");
        }
        
        return _mapper.Map<DetailTransactionDto>(entity);
    }
}