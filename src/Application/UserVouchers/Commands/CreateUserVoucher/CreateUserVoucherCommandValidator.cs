namespace Application.UserVouchers.Commands.CreateUserVoucher;

public class CreateUserVoucherCommandValidator: AbstractValidator<CreateUserVoucherCommand>
{
    public CreateUserVoucherCommandValidator()
    {
        RuleFor(x => x.UserId)
            .NotEmpty().WithMessage("UserId is required")
            .NotNull().WithMessage("UserId is required");
        
        RuleFor(x => x.VoucherId)
            .NotEmpty().WithMessage("VoucherId is required")
            .NotNull().WithMessage("VoucherId is required");
    }
}