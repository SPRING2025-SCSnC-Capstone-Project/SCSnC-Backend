namespace Application.Blogs.Queries.GetBlogById;

public class GetBlogByIdQueryValidator: AbstractValidator<GetBlogByIdQuery>
{
    public GetBlogByIdQueryValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty().WithMessage("Id is required")
            .NotNull().WithMessage("Id is required");
    }
}