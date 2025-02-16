namespace Application.Orders.Commands.UpdateOrder;

public class UpdateOrderCommandValidator : AbstractValidator<UpdateOrderCommand>
{
    public UpdateOrderCommandValidator()
    {
        RuleFor(x => x.OrderId)
            .NotEmpty().WithMessage("OrderId is required.");
        
        RuleFor(x => x.OrderDetails)
            .NotEmpty().WithMessage("OrderDetails is required.");
    }
}