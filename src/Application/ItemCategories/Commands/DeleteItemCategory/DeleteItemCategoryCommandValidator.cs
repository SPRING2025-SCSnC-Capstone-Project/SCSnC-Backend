namespace Application.ItemCategories.Commands.DeleteItemCategory;

public class DeleteItemCategoryCommandValidator: AbstractValidator<DeleteItemCategoryCommand>
{
    public DeleteItemCategoryCommandValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty()
            .WithMessage("Id is required.");
    }
}