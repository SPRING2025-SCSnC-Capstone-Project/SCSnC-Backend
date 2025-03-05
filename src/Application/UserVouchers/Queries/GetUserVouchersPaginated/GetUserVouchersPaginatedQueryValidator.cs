namespace Application.UserVouchers.Queries.GetUserVouchersPaginated;

public class GetUserVouchersPaginatedQueryValidator: AbstractValidator<GetUserVouchersPaginatedQuery>
{
    public GetUserVouchersPaginatedQueryValidator()
    {
        RuleFor(x => x.Page)
            .GreaterThan(-1)
            .WithMessage("Page number can't be negative");

        RuleFor(x => x.Size)
            .GreaterThan(-1)
            .WithMessage("Size can't be negative");
        
        RuleFor(x => x.UserId)
            .NotEmpty().WithMessage("UserId is required")
            .NotNull().WithMessage("UserId is required");
    }
}