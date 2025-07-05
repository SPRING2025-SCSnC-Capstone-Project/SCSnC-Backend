namespace Application.Items.Queries.GetActiveItemById;

public class GetActiveItemByIdQueryValidator: AbstractValidator<GetActiveItemByIdQuery>
{
    public GetActiveItemByIdQueryValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty().WithMessage("ItemId is required")
            .WithMessage("ItemId is not valid");
        
        RuleFor(x => x.BranchId)
            .NotEmpty().WithMessage("BranchId is required")
            .WithMessage("BranchId is not valid");
    }
}