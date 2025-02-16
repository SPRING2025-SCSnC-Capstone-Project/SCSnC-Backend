namespace Application.Sizes.Commands.AddSize;

public class AddSizeCommandValidator: AbstractValidator<AddSizeCommand>
{
    public AddSizeCommandValidator()
    {
        RuleFor(x => x.SizeName)
            .NotEmpty().WithMessage("Size name is required")
            .MaximumLength(200).WithMessage("Size name must not exceed 200 characters");

        RuleFor(x => x.PriceAdjustment)
            .NotNull().WithMessage("Price adjustment is required")
            .GreaterThanOrEqualTo(0).WithMessage("Price adjustment must be greater than or equal 0");
    }
}