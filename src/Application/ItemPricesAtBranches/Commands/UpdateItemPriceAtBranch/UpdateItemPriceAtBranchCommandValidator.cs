namespace Application.ItemPricesAtBranches.Commands.UpdateItemPriceAtBranch;

public class UpdateItemPriceAtBranchCommandValidator: AbstractValidator<UpdateItemPriceAtBranchCommand>
{
    public UpdateItemPriceAtBranchCommandValidator()
    {
        RuleFor(x => x.ItemId)
            .NotEmpty().WithMessage("ItemId is required")
            .NotEqual(Guid.Empty).WithMessage("ItemId is not valid");

        RuleFor(x => x.BranchId)
            .NotEmpty().WithMessage("BranchId is required")
            .NotEqual(Guid.Empty).WithMessage("BranchId is not valid");

        RuleFor(x => x.Price)
            .GreaterThan(0).WithMessage("Price must be greater than 0");
    }
}