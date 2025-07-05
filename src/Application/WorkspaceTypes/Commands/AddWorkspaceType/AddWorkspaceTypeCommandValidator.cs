namespace Application.WorkspaceTypes.Commands;

public class AddWorkspaceTypeCommandValidator : AbstractValidator<AddWorkspaceTypeCommand> {
    public AddWorkspaceTypeCommandValidator() {
        RuleFor(x => x.MaxCapacity)
            .GreaterThan(0).WithMessage("Max capacity must be greater than 0");

        RuleFor(x => x.WorkspaceTypeName)
            .NotEmpty().WithMessage("Workspace type name must not be empty");
    }
}