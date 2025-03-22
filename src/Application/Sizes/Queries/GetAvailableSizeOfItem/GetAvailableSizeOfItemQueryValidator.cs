namespace Application.Sizes.Queries.GetAvailableSizeOfItem;

public class GetAvailableSizeOfItemQueryValidator: AbstractValidator<GetAvailableSizeOfItemQuery>
{
    public GetAvailableSizeOfItemQueryValidator()
    {
        RuleFor(x => x.ItemId)
            .NotEmpty().WithMessage("ItemId is required.");
    }
}