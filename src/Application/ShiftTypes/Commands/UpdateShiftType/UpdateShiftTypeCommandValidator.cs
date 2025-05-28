namespace Application.ShiftTypes.Commands.UpdateShiftType;

public class UpdateShiftTypeCommandValidator: AbstractValidator<UpdateShiftTypeCommand>
{
    public UpdateShiftTypeCommandValidator()
    {
        RuleFor(v => v.Id)
            .NotEmpty().WithMessage("ShiftTypeId is required.")
            .WithMessage("ShiftTypeId is not valid.");
        
        RuleFor(v => v.BranchId)
            .NotEmpty().WithMessage("BranchId is required.")
            .WithMessage("BranchId is not valid.");

        RuleFor(v => v.Name)
            .NotEmpty().WithMessage("Name is required.")
            .MaximumLength(100).WithMessage("Name must not exceed 100 characters.");

        RuleFor(v => v.StartTime)
            .NotEmpty().WithMessage("StartTime is required.")
            .WithMessage("StartTime is not valid.");

        RuleFor(v => v.EndTime)
            .NotEmpty().WithMessage("EndTime is required.")
            .GreaterThan(v => v.StartTime).WithMessage("End time must be greater than start time.");
    }
}