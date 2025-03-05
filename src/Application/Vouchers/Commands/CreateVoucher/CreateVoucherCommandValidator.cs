namespace Application.Vouchers.Commands.CreateVoucher;

public class CreateVoucherCommandValidator : AbstractValidator<CreateVoucherCommand>
{
    public CreateVoucherCommandValidator()
    {
        RuleFor(x => x.VoucherCode)
            .NotEmpty().WithMessage("Voucher code is required.")
            .MaximumLength(50).WithMessage("Voucher code must not exceed 50 characters.");
        RuleFor(x => x.DiscountValue)
            .NotEmpty().WithMessage("Discount value is required.")
            .GreaterThan(0).WithMessage("Discount value must be greater than 0.");
        RuleFor(x => x.ExpiredDate)
            .NotEmpty().WithMessage("Expiry date is required.")
            .GreaterThan(DateTime.Now).WithMessage("Expiry date must be greater than today.");
    }
}