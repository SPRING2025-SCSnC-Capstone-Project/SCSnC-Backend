using Application.Common.Interfaces;
using Application.Common.Models.Dtos;

namespace Application.Vouchers.Commands.DeleteVoucher;

public class DeleteVoucherCommand : IRequest<VoucherDto>
{
    public Guid Id { get; set; }
}

public class DeleteVoucherCommandHandler : IRequestHandler<DeleteVoucherCommand, VoucherDto>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;
    
    public DeleteVoucherCommandHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }
    
    public async Task<VoucherDto> Handle(DeleteVoucherCommand request, CancellationToken cancellationToken)
    {
        var voucher = await _context.Vouchers
            .FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);
        
        if (voucher == null)
        {
            throw new KeyNotFoundException($"Voucher with id {request.Id} not found");
        }
        
        voucher.IsActive = false;
        
        _context.Vouchers.Update(voucher);
        
        await _context.SaveChangesAsync(cancellationToken);
        
        return _mapper.Map<VoucherDto>(voucher);
    }
}