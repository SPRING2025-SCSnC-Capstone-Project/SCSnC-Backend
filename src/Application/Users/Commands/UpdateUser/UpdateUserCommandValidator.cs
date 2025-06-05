namespace Application.Users.Commands;

public class UpdateUserCommandValidator: AbstractValidator<UpdateUserCommand>
{
    public UpdateUserCommandValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty().WithMessage("Id is required")
            .NotNull().WithMessage("Id cannot be null");
        
        RuleFor(x => x.Username)
            .NotEmpty().WithMessage("Username is required")
            .MaximumLength(50).WithMessage("Username must not exceed 50 characters");
        
        // RuleFor(x => x.Password)
        //     .NotEmpty().WithMessage("Password is required")
        //     .MinimumLength(8).WithMessage("Password must be at least 8 characters long");
        //
        //RuleFor(x => x.FullName)
        //    .MaximumLength(100).WithMessage("Full name must not exceed 100 characters");
        
        // RuleFor(x => x.Email)
        //     .NotEmpty().WithMessage("Email is required")
        //     .EmailAddress().WithMessage("Invalid email format");
        //
        //RuleFor(x => x.Phone)
        //    .NotEmpty().WithMessage("Phone number is required")
        //    .MaximumLength(10).WithMessage("Phone number must not exceed 15 characters")
        //    .Matches(@"^0\d{9}$").WithMessage("Phone number must contain only digits");
        
        //RuleFor(x => x.Address)
        //    .MaximumLength(200).WithMessage("Address must not exceed 200 characters");
        
        //RuleFor(x => x.AvatarLink)
        //    .MaximumLength(200).WithMessage("Avatar link must not exceed 200 characters");
        
    }
}