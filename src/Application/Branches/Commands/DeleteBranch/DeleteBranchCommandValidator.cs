namespace Application.Branches.Commands.DeleteBranch;

public class DeleteBranchCommandValidator: AbstractValidator<DeleteBranchCommand>
{
    public DeleteBranchCommandValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty().WithMessage("BranchId is required")
            .NotNull().WithMessage("BranchId is not valid");
    }
}