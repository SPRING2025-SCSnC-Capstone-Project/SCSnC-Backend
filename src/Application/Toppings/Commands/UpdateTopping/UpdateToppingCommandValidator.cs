namespace Application.Toppings.Commands.UpdateTopping;

public class UpdateToppingCommandValidator : AbstractValidator<UpdateToppingCommand>
{
    public UpdateToppingCommandValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty().WithMessage("Topping Id is required");

        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Topping Name is required");
        
        RuleFor(x => x.Description)
            .NotEmpty().WithMessage("Topping Name is required");

        RuleFor(x => x.Price)
            .NotEmpty().WithMessage("Topping Price is required");
    }
}