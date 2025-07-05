namespace Application.UtilityServices.Queries.GetUtilityServicesPaginated;

public class GetUtilityServicesQueryValidator: AbstractValidator<GetUtilityServicesPaginatedQuery>
{
    public GetUtilityServicesQueryValidator()
    {
        RuleFor(v => v.Page)
            .GreaterThan(0).WithMessage("Page number must be greater than 0.");
        
        RuleFor(v => v.Size)
            .GreaterThan(0).WithMessage("Page size must be greater than 0.");
    }
}