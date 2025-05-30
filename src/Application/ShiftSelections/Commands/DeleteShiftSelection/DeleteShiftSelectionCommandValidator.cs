namespace Application.ShiftSelections.Commands.DeleteShiftSelection;

public class DeleteShiftSelectionCommandValidator: AbstractValidator<DeleteShiftSelectionCommand>
{
    public DeleteShiftSelectionCommandValidator()
    {
        RuleFor(x => x.SelectedShiftId)
            .NotEmpty()
            .WithMessage("Selected Shift IDs list cannot be empty");
    }
}