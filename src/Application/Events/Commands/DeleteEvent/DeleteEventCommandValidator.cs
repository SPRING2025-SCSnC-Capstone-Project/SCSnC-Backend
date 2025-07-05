namespace Application.Events.Commands.DeleteEvent;

public class DeleteEventCommandValidator: AbstractValidator<DeleteEventCommand>
{
    public DeleteEventCommandValidator()
    {
        RuleFor(x => x.EventId)
            .NotEmpty()
            .WithMessage("EventId is required.");
    }
}