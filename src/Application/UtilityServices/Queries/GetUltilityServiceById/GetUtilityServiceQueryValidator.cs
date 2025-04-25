namespace Application.UtilityServices.Queries.GetUltilityServiceById;

public class GetUtilityServiceQueryValidator: AbstractValidator<GetUtilityServiceByIdQuery>
{
    public GetUtilityServiceQueryValidator()
    {
        RuleFor(v => v.Id)
            .NotEmpty().WithMessage("Id is required.")
            .WithMessage("Id is not valid.");
    }
}