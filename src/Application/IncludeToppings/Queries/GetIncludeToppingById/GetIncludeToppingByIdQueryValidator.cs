namespace Application.IncludeToppings.Queries.GetIncludeToppingById;

public class GetIncludeToppingByIdQueryValidator: AbstractValidator<GetIncludeToppingByIdQuery>
{
    public GetIncludeToppingByIdQueryValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty()
            .WithMessage("Include Topping Id must not be empty");
    }
}