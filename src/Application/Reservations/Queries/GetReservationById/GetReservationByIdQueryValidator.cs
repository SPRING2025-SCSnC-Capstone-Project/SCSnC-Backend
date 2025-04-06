namespace Application.Reservations.Queries.GetReservationById;

public class GetReservationByIdQueryValidator: AbstractValidator<GetReservationByIdQuery>
{
    public GetReservationByIdQueryValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty()
            .WithMessage("Reservation Id is required.");
    }
}
