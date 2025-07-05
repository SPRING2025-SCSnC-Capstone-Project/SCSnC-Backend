namespace Application.Vouchers.Queries.GetVoucherById;

public class GetVoucherByIdQueryValidator: AbstractValidator<GetVoucherByIdQuery>
{
    public GetVoucherByIdQueryValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty().WithMessage("Id is required")
            .NotNull().WithMessage("Id is required");
    }
}