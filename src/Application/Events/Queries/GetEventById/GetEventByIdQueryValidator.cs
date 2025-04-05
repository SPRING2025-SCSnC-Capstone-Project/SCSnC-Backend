namespace Application.Events.Queries.GetEventById;

public class GetEventByIdQueryValidator: AbstractValidator<GetEventByIdQuery>
{
    public GetEventByIdQueryValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty()
            .WithMessage("Event Id is required.");
    }
}