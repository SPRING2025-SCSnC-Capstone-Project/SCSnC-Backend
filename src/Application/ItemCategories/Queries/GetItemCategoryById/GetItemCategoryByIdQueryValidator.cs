namespace Application.ItemCategories.Queries.GetItemCategoryById;

public class GetItemCategoryByIdQueryValidator: AbstractValidator<GetItemCategoryByIdQuery>
{
    public GetItemCategoryByIdQueryValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty().WithMessage("Id is required")
            .WithMessage("Category is not valid");
    }
}