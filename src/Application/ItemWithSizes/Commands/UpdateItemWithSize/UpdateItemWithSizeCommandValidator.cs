namespace Application.ItemWithSizes.Commands.UpdateItemWithSize;

public class UpdateItemWithSizeCommandValidator : AbstractValidator<UpdateItemWithSizeCommand>
{
    public UpdateItemWithSizeCommandValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty().WithMessage("Item with size Id is required");

        RuleFor(x => x.ItemId)
            .NotEmpty().WithMessage("Item Id is required");

        RuleFor(x => x.SizeId)
            .NotEmpty().WithMessage("Size Id is required");
    }
}