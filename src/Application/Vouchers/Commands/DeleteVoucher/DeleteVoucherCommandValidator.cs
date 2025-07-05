namespace Application.Vouchers.Commands.DeleteVoucher;

public class DeleteVoucherCommandValidator: AbstractValidator<DeleteVoucherCommand>
{
    public DeleteVoucherCommandValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty()
            .WithMessage("Topping Id must not be empty");
    }
}