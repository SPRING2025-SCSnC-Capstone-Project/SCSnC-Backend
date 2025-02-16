namespace Application.Sizes.Commands.DeleteSize;

public class DeleteSizeCommandValidator : AbstractValidator<DeleteSizeCommand>
{
    public DeleteSizeCommandValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty()
            .WithMessage("Size Id must not be empty.");
    }
}