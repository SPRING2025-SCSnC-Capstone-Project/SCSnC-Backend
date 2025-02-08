namespace Application.IncludeToppings.Commands.DeleteIncludeTopping;

public class DeleteIncludeToppingCommandValidator: AbstractValidator<DeleteIncludeToppingCommand>
{
    public DeleteIncludeToppingCommandValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty()
            .WithMessage("Include Topping Id must not be empty");
    }
}