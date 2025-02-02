namespace Application.Tables.Commands;

public class AddTableCommandValidator : AbstractValidator<AddTableCommand>
{
    public AddTableCommandValidator()
    {
        RuleFor(x => x.TableNumber)
            .GreaterThan(0)
            .WithMessage("Table number must be greater than 0");

        RuleFor(x => x.SeatAmount)
            .GreaterThan(0)
            .WithMessage("Seat amount must be greater than 0");
    }
}