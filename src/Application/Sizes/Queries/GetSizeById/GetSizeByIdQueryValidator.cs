namespace Application.Sizes.Queries.GetSizeById;

public class GetSizeByIdQueryValidator : AbstractValidator<GetSizeByIdQuery>
{
    public GetSizeByIdQueryValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty()
            .WithMessage("Size Id must not be empty");
    }
}