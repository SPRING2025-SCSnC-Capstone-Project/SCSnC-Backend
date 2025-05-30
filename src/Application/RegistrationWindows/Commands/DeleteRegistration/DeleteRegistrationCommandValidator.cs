namespace Application.RegistrationWindows.Commands.DeleteRegistration;

public class DeleteRegistrationCommandValidator: AbstractValidator<DeleteResigtrationCommand>
{
    public DeleteRegistrationCommandValidator()
    {
        RuleFor(x => x.RegistrationId)
            .NotEmpty()
            .WithMessage("Registration ID cannot be empty")
            .Must(id => id != Guid.Empty)
            .WithMessage("Registration ID cannot be an empty GUID");
    }
}