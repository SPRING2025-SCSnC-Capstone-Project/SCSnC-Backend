namespace Application.ItemWithSizes.Commands.DeleteItemWithSize;

public class DeleteItemWithSizeCommandValidator : AbstractValidator<DeleteItemWithSizeCommand>
{
    public DeleteItemWithSizeCommandValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty()
            .WithMessage("ItemWithSizeId must not be empty");
    }
}