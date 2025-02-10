namespace Application.WorkspaceTypes.Commands;

public class RemoveWorkspaceTypeCommandValidator : AbstractValidator<RemoveWorkspaceTypeCommand> {
    public RemoveWorkspaceTypeCommandValidator() {
        RuleFor(x => x.Id)
            .NotEmpty().WithMessage("Workspace type Id must not be empty");
    }
}