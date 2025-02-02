namespace Application.Tables.Commands;

public class UpdateTableCommandValidator : AbstractValidator<UpdateTableCommand>
{
    public UpdateTableCommandValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty()
            .WithMessage("Table Id must not be empty");

        RuleFor(x => x.TableNumber)
            .GreaterThan(0)
            .WithMessage("Table number must be greater than 0");

        RuleFor(x => x.SeatAmount)
            .GreaterThan(0)
            .WithMessage("Seat amount must be greater than 0");
    }
}