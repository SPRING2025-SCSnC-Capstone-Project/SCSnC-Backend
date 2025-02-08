namespace Application.Orders.Commands.CreateOrder;

public class CreateOrderCommandValidator: AbstractValidator<CreateOrderCommand>
{
    public CreateOrderCommandValidator()
    {
        RuleFor(x => x.TableId)
            .NotEmpty().WithMessage("TableId is required.");
        
        RuleFor(x => x.UserId)
            .NotEmpty().WithMessage("UserId is required.");
        
        RuleFor(x => x.OrderDetails)
            .NotEmpty().WithMessage("OrderDetails is required.");
    }
}