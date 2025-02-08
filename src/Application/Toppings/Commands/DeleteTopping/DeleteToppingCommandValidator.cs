namespace Application.Toppings.Commands.DeleteTopping;

public class DeleteToppingCommandValidator: AbstractValidator<DeleteToppingCommand>
{
    public DeleteToppingCommandValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty()
            .WithMessage("Topping Id must not be empty");
    }
}