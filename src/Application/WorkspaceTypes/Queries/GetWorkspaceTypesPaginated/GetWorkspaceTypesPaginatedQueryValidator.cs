namespace Application.WorkspaceTypes.Queries;

public class GetWorkspaceTypesPaginatedQueryValidator : AbstractValidator<GetWorkspaceTypesPaginatedQuery>
{
    public GetWorkspaceTypesPaginatedQueryValidator()
    {
        RuleFor(x => x.Page)
            .GreaterThan(-1)
            .WithMessage("Page number can't be negative");

        RuleFor(x => x.Size)
            .GreaterThan(-1)
            .WithMessage("Size can't be negative");
    }
}