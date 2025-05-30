using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Application.Common.Models.Dtos;
using NodaTime;

namespace Application.RegistrationWindows.Commands.UpdateRegistrationWindow;

public record UpdateRegistrationWindowCommand : IRequest<RegistrationWindowDto>
{
    public Guid BranchId { get; init; }
    public DateOnly WeekStart { get; init; }
    public DateTime OpenAt { get; init; }
    public DateTime CloseAt { get; init; }
}

public class UpdateRegistrationWindowCommandHandler : IRequestHandler<UpdateRegistrationWindowCommand, RegistrationWindowDto>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public UpdateRegistrationWindowCommandHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<RegistrationWindowDto> Handle(UpdateRegistrationWindowCommand request, CancellationToken cancellationToken)
    {
        var branch = await _context.Branches.FirstOrDefaultAsync(b => b.Id == request.BranchId && b.IsActive, cancellationToken);

        if (branch is null)
        {
            throw new KeyNotFoundException($"Branch with ID {request.BranchId} not found");
        }

        var window = await _context.RegistrationWindows.FirstOrDefaultAsync(b => b.BranchId == request.BranchId, cancellationToken);

        if (window is null)
        {
            throw new KeyNotFoundException($"Registration window with branch ID {request.BranchId} not found");
        }

        window.WeekStart = LocalDate.FromDateOnly(request.WeekStart);
        window.OpenAt = LocalDateTime.FromDateTime(request.OpenAt);
        window.CloseAt = LocalDateTime.FromDateTime(request.CloseAt);

        _context.RegistrationWindows.Update(window);

        await _context.SaveChangesAsync(cancellationToken);
        
        return _mapper.Map<RegistrationWindowDto>(window);
    }
}
