namespace Application.Branches.Queries.GetBranchById;

public class GetBranchByIdQueryValidator: AbstractValidator<GetBranchByIdQuery>
{
    public GetBranchByIdQueryValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty().WithMessage("BranchId is required")
            .NotEqual(Guid.Empty).WithMessage("BranchId is not valid");
    }
}