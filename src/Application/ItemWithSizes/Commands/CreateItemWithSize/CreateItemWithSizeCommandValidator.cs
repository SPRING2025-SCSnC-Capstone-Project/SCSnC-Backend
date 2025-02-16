using Application.Common.Interfaces;

namespace Application.ItemWithSizes.Commands.CreateItemWithSize;

public class CreateItemWithSizeCommandValidator: AbstractValidator<CreateItemWithSizeCommand>
{
    public CreateItemWithSizeCommandValidator()
    {
        RuleFor(x => x.ItemId)
            .NotEmpty()
            .WithMessage("Item Id must not be empty");

        RuleFor(x => x.SizeId)
            .NotEmpty()
            .WithMessage("Size Id must not be empty");
    }
}