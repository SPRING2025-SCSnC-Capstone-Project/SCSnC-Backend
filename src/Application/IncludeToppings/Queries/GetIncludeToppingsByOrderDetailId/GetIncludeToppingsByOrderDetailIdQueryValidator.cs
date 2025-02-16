namespace Application.IncludeToppings.Queries.GetIncludeToppingsByOrderDetailId;

public class GetIncludeToppingsByOrderDetailIdQueryValidator: AbstractValidator<GetIncludeToppingsByOrderDetailIdQuery>
{
    public GetIncludeToppingsByOrderDetailIdQueryValidator()
    {
        RuleFor(x => x.OrderDetailId)
            .NotEmpty()
            .WithMessage("OrderDetail Id must not be empty");
    }
}