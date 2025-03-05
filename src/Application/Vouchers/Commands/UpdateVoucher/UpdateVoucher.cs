using Application.Common.Interfaces;
using Application.Common.Models.Dtos;
using NodaTime;

namespace Application.Vouchers.Commands.UpdateVoucher;

public record UpdateVoucherCommand : IRequest<VoucherDto>
{
    public Guid Id { get; init; }
    public string VoucherCode { get; init; }
    public int DiscountValue { get; init; }
    public string Description { get; init; }
    public bool IsActive { get; init; }
    public DateTime ExpiredDate { get; init; }
}

public class UpdateVoucherCommandHandler : IRequestHandler<UpdateVoucherCommand, VoucherDto>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;
    
    public UpdateVoucherCommandHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }
    
    public async Task<VoucherDto> Handle(UpdateVoucherCommand request, CancellationToken cancellationToken)
    {
        var voucher = await _context.Vouchers
            .FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);
        
        if (voucher == null)
        {
            throw new KeyNotFoundException($"Voucher with id {request.Id} not found");
        }
        
        voucher.VoucherCode = request.VoucherCode;
        voucher.DiscountValue = request.DiscountValue;
        voucher.Description = request.Description;
        voucher.ExpiredDate = LocalDateTime.FromDateTime(request.ExpiredDate);
        voucher.IsActive = request.IsActive;
        voucher.LastUpdatedAt = LocalDateTime.FromDateTime(DateTime.Now);
        
        _context.Vouchers.Update(voucher);
        
        await _context.SaveChangesAsync(cancellationToken);
        
        return _mapper.Map<VoucherDto>(voucher);
    }
}