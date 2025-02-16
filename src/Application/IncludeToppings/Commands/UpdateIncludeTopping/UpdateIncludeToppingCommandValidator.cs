namespace Application.IncludeToppings.Commands.UpdateIncludeTopping;

public class UpdateIncludeToppingCommandValidator: AbstractValidator<UpdateIncludeToppingCommand>
{
    public UpdateIncludeToppingCommandValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty()
            .WithMessage("Include Topping Id must not be empty");

        RuleFor(x => x.ToppingId)
            .NotEmpty()
            .WithMessage("Topping Id must not be empty");

        RuleFor(x => x.OrderDetailId)
            .NotEmpty()
            .WithMessage("OrderDetail Id must not be empty");
    }
}