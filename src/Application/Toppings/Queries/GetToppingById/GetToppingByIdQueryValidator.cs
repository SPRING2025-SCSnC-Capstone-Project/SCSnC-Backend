namespace Application.Toppings.Queries.GetToppingById;

public class GetToppingByIdQueryValidator : AbstractValidator<GetToppingByIdQuery>
{
    public GetToppingByIdQueryValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty().WithMessage("Topping Id is required");
    }
}