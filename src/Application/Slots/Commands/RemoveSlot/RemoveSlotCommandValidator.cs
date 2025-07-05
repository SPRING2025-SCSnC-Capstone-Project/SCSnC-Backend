namespace Application.Slots.Commands;

public class RemoveSlotCommandValidator : AbstractValidator<RemoveSlotCommand>
{
    public RemoveSlotCommandValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty()
            .WithMessage("Slot Id must not be empty");
    }
}