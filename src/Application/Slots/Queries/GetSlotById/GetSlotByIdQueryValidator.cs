namespace Application.Slots.Queries;

public class GetSlotByIdQueryValidator : AbstractValidator<GetSlotByIdQuery>
{
    public GetSlotByIdQueryValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty()
            .WithMessage("Slot Id must not be empty");
    }
}