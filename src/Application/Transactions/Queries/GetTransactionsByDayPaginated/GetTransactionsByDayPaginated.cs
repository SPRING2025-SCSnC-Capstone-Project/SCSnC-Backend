using Application.Common.Extensions;
using Application.Common.Interfaces;
using Application.Common.Models;
using Application.Common.Models.Dtos;
using Domain.Entities;
using NodaTime;

namespace Application.Transactions.Queries.GetTransactionsByDayPaginated;

public record GetTransactionsByDayPaginatedQuery: IRequest<PaginatedList<TransactionDto>>
{
    public DateTime StartDate { get; init; }
    public DateTime EndDate { get; init; }
    public int? Page { get; init; }
    public int? Size { get; init; }
    public string? SortBy { get; init; }
    public string? SortOrder { get; init; }
}

public class GetTransactionsByDayPaginatedQueryHandler: IRequestHandler<GetTransactionsByDayPaginatedQuery, PaginatedList<TransactionDto>>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;
    
    public GetTransactionsByDayPaginatedQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }
    
    public async Task<PaginatedList<TransactionDto>> Handle(GetTransactionsByDayPaginatedQuery request, CancellationToken cancellationToken)
    {
        var query = _context.Transactions
            .Where(x => x.TransactionDate >= LocalDateTime.FromDateTime(request.StartDate.Add(new TimeSpan(0,0,0))) 
                        && x.TransactionDate <= LocalDateTime.FromDateTime(request.EndDate.Add(new TimeSpan(23,59,59)))
                        && x.TransactionStatus == "Success")
            .AsQueryable();
        
        return await query.ListPaginateWithSortAsync<Transaction, TransactionDto>(
            request.Page, 
            request.Size, 
            request.SortBy, 
            request.SortOrder, 
            _mapper.ConfigurationProvider, 
            cancellationToken
        );
    }
}