using Application.Common.Interfaces;
using Application.Common.Models.Dtos;
using Domain.Entities;
using NodaTime.Extensions;

namespace Application.RegistrationWindows.Commands;

public record CreateRegistrationWindowCommand : IRequest<RegistrationWindowDto>
{
    public DateOnly WeekStart { get; init; }
    public DateTime OpenAt { get; init; }
    public DateTime CloseAt { get; init; }
    public Guid BranchId { get; init; }
}

public class CreateRegistrationWindowCommandHandler : IRequestHandler<CreateRegistrationWindowCommand, RegistrationWindowDto>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public CreateRegistrationWindowCommandHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<RegistrationWindowDto> Handle(CreateRegistrationWindowCommand request, CancellationToken cancellationToken)
    {
        var branch = await _context.Branches.FirstOrDefaultAsync(b => b.Id == request.BranchId, cancellationToken);
        if (branch == null)
        {
            throw new KeyNotFoundException($"Branch with Id {request.BranchId} does not exist");
        }

        var registrationWindow = new RegistrationWindow()
        {
            BranchId = request.BranchId,
            WeekStart = request.WeekStart.ToLocalDate(),
            OpenAt = request.OpenAt.ToLocalDateTime(),
            CloseAt = request.CloseAt.ToLocalDateTime(),
        };

        await _context.RegistrationWindows.AddAsync(registrationWindow, cancellationToken);

        await _context.SaveChangesAsync(cancellationToken);

        return _mapper.Map<RegistrationWindowDto>(registrationWindow);
    }
}

