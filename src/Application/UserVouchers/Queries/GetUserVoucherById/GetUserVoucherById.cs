using Application.Common.Interfaces;
using Application.Common.Models.Dtos;

namespace Application.UserVouchers.Queries.GetUserVoucherById;

public record GetUserVoucherByIdQuery : IRequest<UserVoucherDto>
{
    public Guid Id { get; init; }
}

public class GetUserVoucherByIdQueryHandler : IRequestHandler<GetUserVoucherByIdQuery, UserVoucherDto>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;
    
    public GetUserVoucherByIdQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }
    
    public async Task<UserVoucherDto> Handle(GetUserVoucherByIdQuery request, CancellationToken cancellationToken)
    {
        var userVoucher = await _context.UserVouchers
            .Include(x => x.Voucher)
            .FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);
        
        if (userVoucher is null)
        {
            throw new KeyNotFoundException($"UserVoucher with id {request.Id} not found");
        }
        
        return _mapper.Map<UserVoucherDto>(userVoucher);
    }
}