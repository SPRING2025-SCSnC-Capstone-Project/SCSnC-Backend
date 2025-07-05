namespace Application.IncludeToppings.Commands.CreateIncludeTopping;

public class CreateIncludeToppingCommandValidator: AbstractValidator<CreateIncludeToppingCommand>
{
    public CreateIncludeToppingCommandValidator()
    {
        RuleFor(x => x.ToppingId)
            .NotEmpty()
            .WithMessage("Topping Id must not be empty");

        RuleFor(x => x.OrderDetailId)
            .NotEmpty()
            .WithMessage("Order Detail Id must not be empty");
    }
}