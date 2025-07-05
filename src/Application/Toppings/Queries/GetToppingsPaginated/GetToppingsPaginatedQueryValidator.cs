namespace Application.Toppings.Queries.GetToppingsPaginated;

public class GetToppingsPaginatedQueryValidator : AbstractValidator<GetToppingsPaginatedQuery>
{
    public GetToppingsPaginatedQueryValidator()
    {
        RuleFor(x => x.Page)
            .GreaterThan(-1)
            .WithMessage("Page number can't be negative");

        RuleFor(x => x.Size)
            .GreaterThan(-1)
            .WithMessage("Size can't be negative");
    }
}