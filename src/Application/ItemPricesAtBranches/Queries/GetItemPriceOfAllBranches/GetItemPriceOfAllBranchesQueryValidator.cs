namespace Application.ItemPricesAtBranches.Queries.GetItemPriceOfAllBranches;

public class GetItemPriceOfAllBranchesQueryValidator: AbstractValidator<GetItemPriceOfAllBranchesQuery>
{
    public GetItemPriceOfAllBranchesQueryValidator()
    {
        RuleFor(x => x.ItemId)
            .NotEmpty().WithMessage("ItemId is required")
            .NotEqual(Guid.Empty).WithMessage("ItemId is not valid");
    }
}