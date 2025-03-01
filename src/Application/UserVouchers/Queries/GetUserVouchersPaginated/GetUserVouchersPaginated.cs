using Application.Common.Extensions;
using Application.Common.Interfaces;
using Application.Common.Models;
using Application.Common.Models.Dtos;
using Domain.Entities;

namespace Application.UserVouchers.Queries.GetUserVouchersPaginated;

public record GetUserVouchersPaginatedQuery() : IRequest<PaginatedList<UserVoucherDto>>
{
    public int? Page { get; init; }
    public int? Size { get; init; }
    public string? SortBy { get; init; }
    public string? SortOrder { get; init; }
    public Guid UserId { get; set; }
}

public class GetUserVouchersPaginatedQueryHandler : IRequestHandler<GetUserVouchersPaginatedQuery, PaginatedList<UserVoucherDto>>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;
    
    public GetUserVouchersPaginatedQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }
    
    public async Task<PaginatedList<UserVoucherDto>> Handle(GetUserVouchersPaginatedQuery request, CancellationToken cancellationToken)
    {
        var query = _context.UserVouchers
            .Include(x => x.Voucher)
            .Where(x => x.UserId == request.UserId && x.RedeemStatus == false)
            .AsQueryable();
        
        return await query.ListPaginateWithSortAsync<UserVoucher, UserVoucherDto>(
            request.Page, 
            request.Size, 
            request.SortBy, 
            request.SortOrder, 
            _mapper.ConfigurationProvider, 
            cancellationToken
        );
    }
}