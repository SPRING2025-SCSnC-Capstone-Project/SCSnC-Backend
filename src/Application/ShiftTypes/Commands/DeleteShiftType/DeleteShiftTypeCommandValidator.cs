namespace Application.ShiftTypes.Commands.DeleteShiftType;

public class DeleteShiftTypeCommandValidator: AbstractValidator<DeleteShiftTypeCommand>
{
    public DeleteShiftTypeCommandValidator()
    {
        RuleFor(v => v.ShiftTypeId)
            .NotEmpty().WithMessage("Id is required.")
            .WithMessage("Id is not valid.");
    }
}