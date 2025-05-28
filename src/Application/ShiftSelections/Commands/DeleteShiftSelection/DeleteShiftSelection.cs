using Application.Common.Interfaces;
using Application.Common.Models.Dtos;

namespace Application.ShiftSelections.Commands.DeleteShiftSelection;

public record DeleteShiftSelectionCommand : IRequest<DeleteShiftSelectionResponse>
{
    public List<Guid> SelectedShiftId { get; init; }
}

public class DeleteShiftSelectionCommandHandler: IRequestHandler<DeleteShiftSelectionCommand, DeleteShiftSelectionResponse>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;
    
    public DeleteShiftSelectionCommandHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }
    
    public async Task<DeleteShiftSelectionResponse> Handle(DeleteShiftSelectionCommand request, CancellationToken cancellationToken)
    {
        if (request.SelectedShiftId.Count == 0)
        {
            return new DeleteShiftSelectionResponse
            {
                Message = "No shift selection selected."
            };
        }
        
        foreach (var shiftId in request.SelectedShiftId)
        {
            var selectedShift = await _context.ShiftSelections
                .FirstOrDefaultAsync(x => x.Id == shiftId, cancellationToken);

            if (selectedShift != null)
            {
                throw new KeyNotFoundException($"Selected shift with id {shiftId} not found");
            }
            
            selectedShift.Status = "CANCELED";
            
            _context.ShiftSelections.Remove(selectedShift);
            await _context.SaveChangesAsync(cancellationToken);
        }

        return new DeleteShiftSelectionResponse
        {
            Message = "Selected shifts deleted successfully"
        };
    }
}