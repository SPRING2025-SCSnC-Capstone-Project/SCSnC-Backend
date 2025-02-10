namespace Application.WorkspaceTypes.Queries;

public class GetWorkspaceTypeByIdQueryValidator : AbstractValidator<GetWorkspaceTypeByIdQuery>
{
    public GetWorkspaceTypeByIdQueryValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty()
            .WithMessage("WorkspaceType Id must not be empty");
    }
}