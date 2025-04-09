namespace Application.Branches.Queries.GetBranchesPaginated;

public class GetBranchesPaginatedQueryValidator: AbstractValidator<GetBranchesPaginatedQuery>
{
    public GetBranchesPaginatedQueryValidator()
    {
        RuleFor(x => x.Page)
            .GreaterThan(0).WithMessage("Page number must be greater than 0");
        
        RuleFor(x => x.Size)
            .GreaterThan(0).WithMessage("Page size must be greater than 0");
    }
}