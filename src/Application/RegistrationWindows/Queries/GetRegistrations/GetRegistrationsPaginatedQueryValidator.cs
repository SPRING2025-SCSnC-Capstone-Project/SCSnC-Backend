namespace Application.RegistrationWindows.Queries.GetRegistrations;

public class GetRegistrationsPaginatedQueryValidator: AbstractValidator<GetRegistrationsPaginatedQuery>
{
    public GetRegistrationsPaginatedQueryValidator()
    {
        RuleFor(x => x.Page)
            .GreaterThan(-1)
            .WithMessage("Page number can't be negative");

        RuleFor(x => x.Size)
            .GreaterThan(-1)
            .WithMessage("Size can't be negative");
    }
}