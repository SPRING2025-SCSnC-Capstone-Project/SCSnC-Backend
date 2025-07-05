using Application.Common.Interfaces;
using Application.Common.Models.Dtos;
using Domain.Entities;
using NodaTime;

namespace Application.ShiftTypes.Commands.CreateShiftType;

public record CreateShiftTypeCommand : IRequest<ShiftTypeDto>
{
    public Guid BranchId { get; init; }
    public string Name { get; init; } = null!;
    public TimeOnly StartTime { get; init; }
    public TimeOnly EndTime { get; init; }
}

public class CreateShiftTypeCommandHandler : IRequestHandler<CreateShiftTypeCommand, ShiftTypeDto>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;
    
    public CreateShiftTypeCommandHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }
    
    public async Task<ShiftTypeDto> Handle(CreateShiftTypeCommand request, CancellationToken cancellationToken)
    {
        var entity = new ShiftType
        {
            BranchId = request.BranchId,
            Name = request.Name,
            StartTime = LocalTime.FromTimeOnly(request.StartTime),
            EndTime = LocalTime.FromTimeOnly(request.EndTime),
            IsActive = true,
        };
        
        await _context.ShiftTypes.AddAsync(entity, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
        
        var result = _context.ShiftTypes
            .Include(x => x.Branch)
            .FirstOrDefault(x => x.Id == entity.Id);
        
        return _mapper.Map<ShiftTypeDto>(result);
    }
}