namespace Application.ItemCategories.Queries.GetItemCategoriesPaginated;

public class GetItemsCategoryPaginatedQueryValidator: AbstractValidator<GetItemCategoriesPaginatedQuery>
{
    public GetItemsCategoryPaginatedQueryValidator()
    {
        RuleFor(x => x.Page)
            .GreaterThan(-1)
            .WithMessage("Page number can't be negative");

        RuleFor(x => x.Size)
            .GreaterThan(-1)
            .WithMessage("Size can't be negative");
    }
}