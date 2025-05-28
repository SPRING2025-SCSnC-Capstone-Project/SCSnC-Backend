using Application.Common.Interfaces;
using Application.Common.Models.Dtos;
using Ardalis.GuardClauses;

namespace Application.ShiftTypes.Commands.DeleteShiftType;

public record DeleteShiftTypeCommand : IRequest<ReturnShiftTypeDto>
{
    public Guid ShiftTypeId { get; init; }
}

public class DeleteShiftTypeCommandHandler : IRequestHandler<DeleteShiftTypeCommand, ReturnShiftTypeDto>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public DeleteShiftTypeCommandHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<ReturnShiftTypeDto> Handle(DeleteShiftTypeCommand request, CancellationToken cancellationToken)
    {
        var shiftType = await _context.ShiftTypes
            .Include(x => x.Branch)
            .FirstOrDefaultAsync(x => x.Id == request.ShiftTypeId, cancellationToken);

        if (shiftType == null)
        {
            throw new NotFoundException(nameof(shiftType), request.ShiftTypeId.ToString());
        }

        shiftType.IsActive = false;
        _context.ShiftTypes.Update(shiftType);
        await _context.SaveChangesAsync(cancellationToken);

        return _mapper.Map<ReturnShiftTypeDto>(shiftType);
    }
}