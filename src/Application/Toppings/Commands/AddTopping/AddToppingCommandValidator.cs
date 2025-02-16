namespace Application.Toppings.Commands.AddTopping;

public class AddToppingCommandValidator : AbstractValidator<AddToppingCommand>
{
    public AddToppingCommandValidator()
    {
        RuleFor(x => x.ToppingName)
            .NotEmpty()
            .WithMessage("Topping name must not be empty");
        
        RuleFor(x => x.ToppingDescription)
            .NotEmpty()
            .WithMessage("Topping description must not be empty");

        RuleFor(x => x.Price)
            .GreaterThan(0)
            .WithMessage("Price must be greater than 0");
    }
}