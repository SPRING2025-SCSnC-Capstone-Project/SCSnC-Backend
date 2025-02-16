namespace Application.Sizes.Queries.GetSizesPaginated;

public class GetSizesPaginatedQueryValidator: AbstractValidator<GetSizesPaginatedQuery>
{
    public GetSizesPaginatedQueryValidator()
    {
        RuleFor(x => x.Page)
            .GreaterThan(-1)
            .WithMessage("Page number can't be negative");

        RuleFor(x => x.Size)
            .GreaterThan(-1)
            .WithMessage("Size can't be negative");
    }
}