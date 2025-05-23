namespace Application.Workspaces.Commands;

public class AddWorkspaceCommandValidator : AbstractValidator<AddWorkspaceCommand>
{
    public AddWorkspaceCommandValidator()
    {
        RuleFor(x => x.WorkspaceNumber)
            .GreaterThan(0)
            .WithMessage("Workspace number must be greater than 0");
    }
}