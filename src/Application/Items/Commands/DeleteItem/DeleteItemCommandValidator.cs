namespace Application.Items.Commands.DeleteItem;

public class DeleteItemCommandValidator : AbstractValidator<DeleteItemCommand>
{
    public DeleteItemCommandValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty()
            .WithMessage("Item Id must not be empty.");
    }
}