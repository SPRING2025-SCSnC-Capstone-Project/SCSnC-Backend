namespace Application.UserVouchers.Queries.GetUserVoucherById;

public class GetUserVoucherByIdQueryValidator: AbstractValidator<GetUserVoucherByIdQuery>
{
    public GetUserVoucherByIdQueryValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty().WithMessage("Id is required")
            .NotNull().WithMessage("Id is required");
    }
}