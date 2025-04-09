namespace Application.ToppingPricesAtBranches.Commands.UpdateToppingPriceAtBranch;

public class UpdateToppingPriceAtBranchCommandValidator: AbstractValidator<UpdateToppingPriceAtBranchCommand>
{
    public UpdateToppingPriceAtBranchCommandValidator()
    {
        RuleFor(x => x.ToppingId)
            .NotEmpty().WithMessage("ToppingId is required")
            .NotEqual(Guid.Empty).WithMessage("ToppingId is not valid");

        RuleFor(x => x.BranchId)
            .NotEmpty().WithMessage("BranchId is required")
            .NotEqual(Guid.Empty).WithMessage("BranchId is not valid");

        RuleFor(x => x.Price)
            .GreaterThan(0).WithMessage("Price must be greater than 0");
    }
}