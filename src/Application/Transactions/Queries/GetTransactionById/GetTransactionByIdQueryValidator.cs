namespace Application.Transactions.Queries.GetTransactionById;

public class GetTransactionByIdQueryValidator: AbstractValidator<GetTransactionByIdQuery>
{
    public GetTransactionByIdQueryValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty().WithMessage("Id is required")
            .NotNull().WithMessage("Id is required");
    }
}