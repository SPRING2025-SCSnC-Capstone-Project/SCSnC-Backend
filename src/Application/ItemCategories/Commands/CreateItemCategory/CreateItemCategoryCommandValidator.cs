namespace Application.ItemCategories.Commands.CreateItemCategory;

public class CreateItemCategoryCommandValidator: AbstractValidator<CreateItemCategoryCommand>
{
    public CreateItemCategoryCommandValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Name is required")
            .MaximumLength(50).WithMessage("Name must not exceed 50 characters");
    }
}