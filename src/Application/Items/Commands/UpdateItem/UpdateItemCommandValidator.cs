namespace Application.Items.Commands.UpdateItem;

public class UpdateItemCommandValidator : AbstractValidator<UpdateItemCommand>
{
    public UpdateItemCommandValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Name is required")
            .MaximumLength(200).WithMessage("Name must not exceed 200 characters");

        RuleFor(x => x.Description)
            .MaximumLength(1000).WithMessage("Description must not exceed 1000 characters");

        RuleFor(x => x.CategoryId)
            .NotEmpty().WithMessage("Category is required")
            .WithMessage("Category is not valid");

        RuleFor(x => x.Id)
            .NotEmpty().WithMessage("Id is required")
            .WithMessage("Item is not valid");
        
        RuleFor(x => x.IsActive)
            .NotNull().WithMessage("IsActive is required");
    }
}