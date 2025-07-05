namespace Application.Workspaces.Queries;

public class GetWorkspaceByIdQueryValidator : AbstractValidator<GetWorkspaceByIdQuery>
{
    public GetWorkspaceByIdQueryValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty()
            .WithMessage("Workspace Id must not be empty");
    }
}