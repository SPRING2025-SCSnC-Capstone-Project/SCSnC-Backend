namespace Application.Items.Queries.GetItemById;

public class GetItemByIdQueryValidator : AbstractValidator<GetItemByIdQuery>
{
    public GetItemByIdQueryValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty().WithMessage("Id is required")
            .WithMessage("Item is not valid");
    }
}