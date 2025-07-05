namespace Application.Vouchers.Queries.GetVouchersPaginated;

public class GetVouchersPaginatedQueryValidator: AbstractValidator<GetVouchersPaginatedQuery>
{
    public GetVouchersPaginatedQueryValidator()
    {
        RuleFor(x => x.Page)
            .GreaterThan(-1)
            .WithMessage("Page number can't be negative");

        RuleFor(x => x.Size)
            .GreaterThan(-1)
            .WithMessage("Size can't be negative");
    }
}