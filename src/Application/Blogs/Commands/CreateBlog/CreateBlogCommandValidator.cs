namespace Application.Blogs.Commands.CreateBlog;

public class CreateBlogCommandValidator: AbstractValidator<CreateBlogCommand>
{
    public CreateBlogCommandValidator()
    {
        RuleFor(x => x.EventId)
            .NotEmpty().WithMessage("EventId is required")
            .NotNull().WithMessage("EventId is required");
        
        RuleFor(x => x.UserId)
            .NotEmpty().WithMessage("UserId is required")
            .NotNull().WithMessage("UserId is required");
        
        RuleFor(x => x.Title)
            .NotEmpty().WithMessage("Title is required")
            .NotNull().WithMessage("Title is required");
        
        RuleFor(x => x.Content)
            .NotEmpty().WithMessage("Content is required")
            .NotNull().WithMessage("Content is required");
        
        RuleFor(x => x.Media)
            .NotEmpty().WithMessage("Media is required")
            .NotNull().WithMessage("Media is required");
    }
}