namespace Application.Tables.Commands;

public class RemoveTableCommandValidator : AbstractValidator<RemoveTableCommand>
{
    public RemoveTableCommandValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty()
            .WithMessage("Table Id must not be empty");
    }
}