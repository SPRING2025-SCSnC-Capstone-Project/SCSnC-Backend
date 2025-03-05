namespace Application.Transactions.Queries.GetTransactionsByDayPaginated;

public class GetTransactionsByDayPaginatedQueryValidator: AbstractValidator<GetTransactionsByDayPaginatedQuery>
{
    public GetTransactionsByDayPaginatedQueryValidator()
    {
        RuleFor(x => x.StartDate)
            .NotEmpty().WithMessage("Date is required")
            .NotNull().WithMessage("Date is required");
        
        RuleFor(x => x.EndDate)
            .NotEmpty().WithMessage("Date is required")
            .NotNull().WithMessage("Date is required");
        
        RuleFor(x => x.Page)
            .GreaterThan(-1)
            .WithMessage("Page number can't be negative");

        RuleFor(x => x.Size)
            .GreaterThan(-1)
            .WithMessage("Size can't be negative");
    }
}