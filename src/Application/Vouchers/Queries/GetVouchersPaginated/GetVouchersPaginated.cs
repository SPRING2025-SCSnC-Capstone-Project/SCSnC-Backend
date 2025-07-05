using Application.Common.Extensions;
using Application.Common.Interfaces;
using Application.Common.Models;
using Application.Common.Models.Dtos;
using Domain.Entities;

namespace Application.Vouchers.Queries.GetVouchersPaginated;

public record GetVouchersPaginatedQuery : IRequest<PaginatedList<VoucherDto>>
{
    public int? Page { get; init; }
    public int? Size { get; init; }
    public string? SortBy { get; init; }
    public string? SortOrder { get; init; }
}

public class GetVouchersPaginatedQueryHandler : IRequestHandler<GetVouchersPaginatedQuery, PaginatedList<VoucherDto>>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;
    
    public GetVouchersPaginatedQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }
    
    public async Task<PaginatedList<VoucherDto>> Handle(GetVouchersPaginatedQuery request, CancellationToken cancellationToken)
    {
        var query = _context.Vouchers.AsQueryable();
        
        return await query.ListPaginateWithSortAsync<Voucher, VoucherDto>(
            request.Page, 
            request.Size, 
            request.SortBy, 
            request.SortOrder, 
            _mapper.ConfigurationProvider, 
            cancellationToken
        );
    }
}