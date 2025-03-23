namespace Application.Blogs.Queries.GetBlogsPaginated;

public class GetBlogsPaginatedQueryValidator: AbstractValidator<GetBlogsPaginatedQuery>
{
    public GetBlogsPaginatedQueryValidator()
    {
        RuleFor(x => x.Page)
            .GreaterThan(-1)
            .WithMessage("Page number can't be negative");

        RuleFor(x => x.Size)
            .GreaterThan(-1)
            .WithMessage("Size can't be negative");
    }
}