namespace Application.Workspaces.Commands;

public class UpdateWorkspaceCommandValidator : AbstractValidator<UpdateWorkspaceCommand>
{
    public UpdateWorkspaceCommandValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty()
            .WithMessage("Workspace Id must not be empty");

        RuleFor(x => x.WorkspaceNumber)
            .GreaterThan(0)
            .WithMessage("Workspace number must be greater than 0");

        RuleFor(x => x.WorkspaceTypeId)
            .NotEmpty()
            .WithMessage("Workspace type Id must not be empty");
    }
}