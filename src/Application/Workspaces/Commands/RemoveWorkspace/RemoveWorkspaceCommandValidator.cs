namespace Application.Workspaces.Commands;

public class RemoveWorkspaceCommandValidator : AbstractValidator<RemoveWorkspaceCommand>
{
    public RemoveWorkspaceCommandValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty()
            .WithMessage("Workspace Id must not be empty");
    }
}