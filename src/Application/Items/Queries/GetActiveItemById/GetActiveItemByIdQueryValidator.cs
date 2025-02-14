namespace Application.Items.Queries.GetActiveItemById;

public class GetActiveItemByIdQueryValidator: AbstractValidator<GetActiveItemByIdQuery>
{
    public GetActiveItemByIdQueryValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty().WithMessage("Id is required")
            .WithMessage("Item is not valid");
    }
}