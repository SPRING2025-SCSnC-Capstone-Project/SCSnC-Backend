namespace Application.Blogs.Commands.DeleteBlog;

public class DeleteBlogCommandValidator: AbstractValidator<DeleteBlogCommand>
{
    public DeleteBlogCommandValidator()
    {
        RuleFor(x => x.BlogId)
            .NotEmpty().WithMessage("BlogId is required")
            .NotNull().WithMessage("BlogId is required");
    }
}