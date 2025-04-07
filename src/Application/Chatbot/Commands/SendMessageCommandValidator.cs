namespace Application.Chatbot.Commands;

public class SendMessageCommandValidator: AbstractValidator<SendMessageCommand>
{
    public SendMessageCommandValidator()
    {
        RuleFor(x => x.Request).NotEmpty();
    }
}