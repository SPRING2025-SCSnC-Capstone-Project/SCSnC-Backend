namespace Application.UtilityServices.Commands.UpdateUtilityService;

public class UpdateUtilityServiceCommandValidator: AbstractValidator<UpdateUtilityServiceCommand>
{
    public UpdateUtilityServiceCommandValidator()
    {
        RuleFor(v => v.Id)
            .NotEmpty().WithMessage("Id is required.")
            .WithMessage("Id is not valid.");
        
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