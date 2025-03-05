namespace Application.Vouchers.Commands.UpdateVoucher;

public class UpdateVoucherCommandValidator: AbstractValidator<UpdateVoucherCommand>
{
    public UpdateVoucherCommandValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty().WithMessage("Voucher Id is required");
        
        RuleFor(x => x.DiscountValue)
            .NotEmpty().WithMessage("Discount value is required")
            .GreaterThan(0).WithMessage("Discount value must be greater than 0");
        
        RuleFor(x => x.VoucherCode)
            .NotEmpty().WithMessage("Voucher code is required");
        
        RuleFor(x => x.ExpiredDate)
            .NotEmpty().WithMessage("Expired date is required")
            .GreaterThan(DateTime.Now).WithMessage("Expired date must be greater than current date");
    }
}