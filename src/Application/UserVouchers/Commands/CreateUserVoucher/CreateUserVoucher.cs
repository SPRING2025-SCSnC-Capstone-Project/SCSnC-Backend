using Application.Common.Interfaces;
using Application.Common.Models.Dtos;
using Domain.Entities;
using NodaTime;

namespace Application.UserVouchers.Commands.CreateUserVoucher;

public record CreateUserVoucherCommand : IRequest<UserVoucherDto>
{
    public Guid UserId { get; init; }
    public Guid VoucherId { get; init; }
}

public class CreateUserVoucherCommandHandler : IRequestHandler<CreateUserVoucherCommand, UserVoucherDto>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;
    
    public CreateUserVoucherCommandHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }
    
    public async Task<UserVoucherDto> Handle(CreateUserVoucherCommand request, CancellationToken cancellationToken)
    {
        var userVoucher = new UserVoucher
        {
            UserId = request.UserId,
            VoucherId = request.VoucherId,
            DateAdded = LocalDateTime.FromDateTime(DateTime.Now),
            RedeemStatus = false
        };
        
        _context.UserVouchers.Add(userVoucher);
        await _context.SaveChangesAsync(cancellationToken);
        
        var result = await _context.UserVouchers
            .Include(x => x.Voucher)
            .FirstOrDefaultAsync(x => x.Id == userVoucher.Id, cancellationToken);
        
        return _mapper.Map<UserVoucherDto>(result);
    }
}