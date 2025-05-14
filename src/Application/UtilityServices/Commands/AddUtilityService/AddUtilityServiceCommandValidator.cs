namespace Application.UtilityServices.Commands.AddUtilityService;

public class AddUtilityServiceCommandValidator: AbstractValidator<AddUtilityServiceCommand>
{
    public AddUtilityServiceCommandValidator()
    {
        RuleFor(v => v.Name)
            .NotEmpty().WithMessage("Name is required.")
            .MaximumLength(100).WithMessage("Name must not exceed 100 characters.");

        RuleFor(v => v.ImgUrl)
            .NotEmpty().WithMessage("Image URL is required.")
            .WithMessage("Image URL is not valid.");
        
        RuleFor(v => v.ServiceFee)
            .GreaterThan(0).WithMessage("Service fee must be greater than 0.")
            .WithMessage("Service fee is not valid.");
    }
}