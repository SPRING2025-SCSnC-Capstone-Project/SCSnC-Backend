namespace Application.Items.Commands.AddItem;

public class AddItemCommandValidator : AbstractValidator<AddItemCommand>
{
    public AddItemCommandValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Name is required")
            .MaximumLength(200).WithMessage("Name must not exceed 200 characters");

        RuleFor(x => x.Description)
            .MaximumLength(1000).WithMessage("Description must not exceed 1000 characters");

        RuleFor(x => x.Price)
            .GreaterThan(0).WithMessage("Price must be greater than 0");

        RuleFor(x => x.CategoryId)
            .NotEmpty().WithMessage("Category is required")
            .WithMessage("Category is not valid");
        
        RuleFor(x => x.Img)
            .NotEmpty().WithMessage("Image URL is required")
            .WithMessage("Image URL is not valid");
    }
}