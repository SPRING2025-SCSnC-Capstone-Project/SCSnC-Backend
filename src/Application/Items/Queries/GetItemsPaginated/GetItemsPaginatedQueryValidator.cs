using Application.Items.Queries.GetItemById;

namespace Application.Items.Queries.GetItemsPaginated;

public class GetItemsPaginatedQueryValidator : AbstractValidator<GetItemsPaginatedQuery>
{
    public GetItemsPaginatedQueryValidator()
    {
        RuleFor(x => x.Page)
            .GreaterThan(-1)
            .WithMessage("Page number can't be negative");

        RuleFor(x => x.Size)
            .GreaterThan(-1)
            .WithMessage("Size can't be negative");
    }
}