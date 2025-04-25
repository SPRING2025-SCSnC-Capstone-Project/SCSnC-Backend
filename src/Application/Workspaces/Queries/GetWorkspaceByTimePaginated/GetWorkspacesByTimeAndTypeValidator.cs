namespace Application.Workspaces.Queries;

public class GetWorkspaceByTimePaginatedValidator : AbstractValidator<GetWorkspacesByTimeAndTypeQuery>
{
    public GetWorkspaceByTimePaginatedValidator()
    {
        RuleFor(x => x.ReservationDate)
            .NotEmpty()
            .WithMessage("Reserve date cant be empty");

        RuleFor(x => x.WorkspaceTypeId)
            .NotEmpty()
            .WithMessage("Workspace type Id must not be empty");
    }
}