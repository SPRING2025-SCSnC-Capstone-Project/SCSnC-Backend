using Application.Common.Interfaces;
using Application.Common.Models.Dtos;
using Ardalis.GuardClauses;
using NodaTime;

namespace Application.ShiftTypes.Commands.UpdateShiftType;

public record UpdateShiftTypeCommand : IRequest<ShiftTypeDto>
{
    public Guid Id { get; init; }
    public Guid BranchId { get; init; }
    public string Name { get; init; } = null!;
    public TimeOnly StartTime { get; init; }
    public TimeOnly EndTime { get; init; }
}

public class UpdateShiftTypeCommandHandler : IRequestHandler<UpdateShiftTypeCommand, ShiftTypeDto>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;
    
    public UpdateShiftTypeCommandHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }
    
    public async Task<ShiftTypeDto> Handle(UpdateShiftTypeCommand request, CancellationToken cancellationToken)
    {
        var entity = await _context.ShiftTypes
            .FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);

        if (entity == null)
        {
            throw new NotFoundException(nameof(entity), request.Id.ToString());
        }
        
        entity.BranchId = request.BranchId;
        entity.Name = request.Name;
        entity.StartTime = LocalTime.FromTimeOnly(request.StartTime);
        entity.EndTime = LocalTime.FromTimeOnly(request.EndTime);
        
        _context.ShiftTypes.Update(entity);
        await _context.SaveChangesAsync(cancellationToken);
        
        var result = _context.ShiftTypes
            .Include(x => x.Branch)
            .FirstOrDefault(x => x.Id == entity.Id);
        
        return _mapper.Map<ShiftTypeDto>(result);
    }
}