namespace Application.ShiftTypes.Commands.CreateShiftType;

public class CreateShiftTypeCommandValidator: AbstractValidator<CreateShiftTypeCommand>
{
    public CreateShiftTypeCommandValidator()
    {
        RuleFor(v => v.Name)
            .NotEmpty().WithMessage("Name is required.")
            .MaximumLength(100).WithMessage("Name must not exceed 100 characters.");

        RuleFor(v => v.StartTime)
            .NotEmpty().WithMessage("Start time is required.");

        RuleFor(v => v.EndTime)
            .NotEmpty().WithMessage("End time is required.")
            .GreaterThan(v => v.StartTime).WithMessage("End time must be greater than start time.");
    }
}