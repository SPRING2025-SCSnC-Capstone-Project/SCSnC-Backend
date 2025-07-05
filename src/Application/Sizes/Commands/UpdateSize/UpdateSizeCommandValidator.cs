namespace Application.Sizes.Commands.UpdateSize;

public class UpdateSizeCommandValidator: AbstractValidator<UpdateSizeCommand>
{
    public UpdateSizeCommandValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty().WithMessage("Size Id is required");

        RuleFor(x => x.SizeName)
            .NotEmpty().WithMessage("Size name is required")
            .MaximumLength(200).WithMessage("Size name must not exceed 200 characters");

        RuleFor(x => x.PriceAdjustment)
            .NotEmpty().WithMessage("Price adjustment is required")
            .GreaterThanOrEqualTo(0).WithMessage("Price adjustment must be greater than 0");
    }
}