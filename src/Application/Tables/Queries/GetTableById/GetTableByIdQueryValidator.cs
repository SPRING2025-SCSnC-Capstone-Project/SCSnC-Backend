namespace Application.Tables.Queries;

public class GetTableByIdQueryValidator : AbstractValidator<GetTableByIdQuery>
{
    public GetTableByIdQueryValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty()
            .WithMessage("Table Id must not be empty");
    }
}