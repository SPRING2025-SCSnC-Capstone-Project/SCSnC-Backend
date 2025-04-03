namespace Application.Blogs.Commands.UpdateBlog;

public class UpdateBlogCommandValidator: AbstractValidator<UpdateBlogCommand>
{
    public UpdateBlogCommandValidator()
    {
        RuleFor(x => x.BlogId)
            .NotEmpty().WithMessage("BlogId is required")
            .NotNull().WithMessage("BlogId is required");
        
        RuleFor(x => x.Title)
            .NotEmpty().WithMessage("Title is required")
            .NotNull().WithMessage("Title is required");
        
        RuleFor(x => x.Content)
            .NotEmpty().WithMessage("Content is required")
            .NotNull().WithMessage("Content is required");
    }
}