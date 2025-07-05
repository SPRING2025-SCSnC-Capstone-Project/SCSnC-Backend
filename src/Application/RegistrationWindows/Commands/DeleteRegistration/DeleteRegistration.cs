using Application.Common.Interfaces;
using Application.Common.Models.Dtos;

namespace Application.RegistrationWindows.Commands.DeleteRegistration;

public record DeleteResigtrationCommand : IRequest<DeleteRegistrationWindowResponse>
{
    public Guid RegistrationId { get; init; }
}

public class DeleteRegistrationCommandHandler : IRequestHandler<DeleteResigtrationCommand, DeleteRegistrationWindowResponse>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;
    
    public DeleteRegistrationCommandHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }
    
    public async Task<DeleteRegistrationWindowResponse> Handle(DeleteResigtrationCommand request, CancellationToken cancellationToken)
    {
        var registrationWindow = await _context.RegistrationWindows
            .FirstOrDefaultAsync(x => x.Id == request.RegistrationId, cancellationToken);

        if (registrationWindow == null)
        {
            throw new KeyNotFoundException($"Registration window with id {request.RegistrationId} not found");
        }

        _context.RegistrationWindows.Remove(registrationWindow);
        await _context.SaveChangesAsync(cancellationToken);

        return new DeleteRegistrationWindowResponse
        {
            Message = "Registration window deleted successfully"
        };
    }
}