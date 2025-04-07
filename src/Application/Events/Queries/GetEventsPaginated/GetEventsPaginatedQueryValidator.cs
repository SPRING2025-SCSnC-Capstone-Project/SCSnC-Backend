namespace Application.Events.Queries.GetEventsPaginated;

public class GetEventsPaginatedQueryValidator: AbstractValidator<GetEventsPaginatedQuery>
{
    public GetEventsPaginatedQueryValidator()
    {
        RuleFor(x => x.Page)
            .GreaterThanOrEqualTo(0)
            .WithMessage("Page index must be greater than or equal to 0.");

        RuleFor(x => x.Size)
            .GreaterThan(0)
            .WithMessage("Page size must be greater than 0.");
    }
}