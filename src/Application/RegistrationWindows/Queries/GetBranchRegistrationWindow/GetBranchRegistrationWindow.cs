using Application.Common.Interfaces;
using Application.Common.Models.Dtos;

namespace Application.RegistrationWindows.Queries.GetBranchRegistrationWindow;

public record GetBranchRegistrationWindowQuery : IRequest<RegistrationWindowDto>
{
    public Guid BranchId { get; init; }
    public DateOnly WeekStart { get; init; }
}

public class GetBranchRegistrationWindowQueryHandler : IRequestHandler<GetBranchRegistrationWindowQuery, RegistrationWindowDto>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetBranchRegistrationWindowQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<RegistrationWindowDto> Handle(GetBranchRegistrationWindowQuery request, CancellationToken cancellationToken){
        var registrationWindow = await _context.RegistrationWindows.FirstOrDefaultAsync(x => x.BranchId == request.BranchId 
            && x.WeekStart.ToDateOnly() == request.WeekStart, cancellationToken);

        if (registrationWindow == null)
        {
            throw new KeyNotFoundException("Registration window not found");
        }

        return _mapper.Map<RegistrationWindowDto>(registrationWindow);
    }
}

