using Application.Common.Interfaces;
using Application.Common.Models.Dtos;

namespace Application.Vouchers.Queries.GetVoucherById;

public record GetVoucherByIdQuery() : IRequest<VoucherDto>
{
    public Guid Id { get; set; }
}

public class GetVoucherByIdQueryHandler : IRequestHandler<GetVoucherByIdQuery, VoucherDto>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;
    
    public GetVoucherByIdQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }
    
    public async Task<VoucherDto> Handle(GetVoucherByIdQuery request, CancellationToken cancellationToken)
    {
        var entity = await _context.Vouchers
            .FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);
        
        if (entity == null)
        {
            throw new KeyNotFoundException($"Topping with id {request.Id} not found");
        }
        
        return _mapper.Map<VoucherDto>(entity);
    }
}