namespace Application.Slots.Commands;

public class UpdateSlotCommandValidator : AbstractValidator<UpdateSlotCommand>
{
    public UpdateSlotCommandValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty()
            .WithMessage("Slot Id must not be empty");

        RuleFor(x => x.SlotNumber)
            .GreaterThan(0)
            .WithMessage("Slot number must be greater than 0");

        RuleFor(x => x.TimeEnd)
            .GreaterThan(x => x.TimeStart)
            .WithMessage("End Time can't be before Start Time");
    }
}