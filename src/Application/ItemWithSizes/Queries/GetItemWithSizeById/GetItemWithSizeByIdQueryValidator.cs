namespace Application.ItemWithSizes.Queries.GetItemWithSizeById;

public class GetItemWithSizeByIdQueryValidator: AbstractValidator<GetItemWithSizeByIdQuery>
{
    public GetItemWithSizeByIdQueryValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty()
            .WithMessage("ItemWithSize Id must not be empty");
    }
}