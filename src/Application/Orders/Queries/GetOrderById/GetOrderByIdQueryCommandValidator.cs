namespace Application.Orders.Queries.GetOrderById;

public class GetOrderByIdQueryCommandValidator: AbstractValidator<GetOrderByIdQuery>
{
    public GetOrderByIdQueryCommandValidator()
    {
        RuleFor(x => x.OrderId)
            .NotEmpty().WithMessage("OrderId is required.");
    }
}