using Application.Common.Interfaces;
using Application.Common.Models.Dtos;
using Domain.Entities;
using NodaTime;

namespace Application.Vouchers.Commands.CreateVoucher;

public record CreateVoucherCommand : IRequest<VoucherDto>
{
    public string VoucherCode { get; init; }
    public int DiscountValue { get; init; }
    public string Description { get; init; }
    public DateTime ExpiredDate { get; init; }
}

public class CreateVoucherCommandHandler : IRequestHandler<CreateVoucherCommand, VoucherDto>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public CreateVoucherCommandHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<VoucherDto> Handle(CreateVoucherCommand request, CancellationToken cancellationToken)
    {
        var voucher = new Voucher
        {
            VoucherCode = request.VoucherCode,
            DiscountValue = request.DiscountValue,
            ExpiredDate = LocalDateTime.FromDateTime(request.ExpiredDate),
            Description = request.Description,
            IsActive = true,
            CreatedAt = LocalDateTime.FromDateTime(DateTime.Now),
            LastUpdatedAt = LocalDateTime.FromDateTime(DateTime.Now)
        };

        var result = await _context.Vouchers.AddAsync(voucher);
        
        await _context.SaveChangesAsync(cancellationToken);

        return _mapper.Map<VoucherDto>(result);
    }
}