namespace Application.WorkspaceTypes.Commands;

public class UpdateWorkspaceTypeCommandValidator : AbstractValidator<UpdateWorkspaceTypeCommand> {
    public UpdateWorkspaceTypeCommandValidator() {
        RuleFor(x => x.Id)
            .NotEmpty().WithMessage("Workspace type Id must not be empty");
            
        RuleFor(x => x.MaxCapacity)
            .GreaterThan(0).WithMessage("Max capacity must be greater than 0");

        RuleFor(x => x.WorkspaceTypeName)
            .NotEmpty().WithMessage("Workspace type name must not be empty");
    }

    
}