using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Application.Common.Models.Dtos;
using Domain.Entities;
using Microsoft.AspNetCore.Http;
using NodaTime;

namespace Application.Events.Commands;

public record UpdateEventCommand : IRequest<EventDto>
{
    public Guid EventId { get; set; }
    public string? EventTitle { get; init; } = null!;
    public IFormFile? Image { get; set; }
    public bool IsPrivate { get; set; }

}

public class UpdateEventCommandHandler : IRequestHandler<UpdateEventCommand, EventDto>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;
    private readonly IAzureService _azureService;

    public UpdateEventCommandHandler(IMapper mapper, IApplicationDbContext context, IAzureService azureService)
    {
        _mapper = mapper;
        _context = context;
        _azureService = azureService;
    }

    public async Task<EventDto> Handle(UpdateEventCommand request, CancellationToken cancellationToken)
    {
        var existedEvent = await _context.Events
            .Include(x => x.Reservation)
                .ThenInclude(y => y.Workspace)
                .ThenInclude(z => z.WorkspaceTypeAtBranch)
                .ThenInclude(w => w.WorkspaceType)
            .Include(x => x.Reservation)
                .ThenInclude(y => y.Workspace)
                .ThenInclude(z => z.WorkspaceTypeAtBranch)
                .ThenInclude(w => w.Branch)
            .AsNoTracking()
            .Include(x => x.EventSlots)
                .ThenInclude(y => y.Slot)
            .FirstOrDefaultAsync(x => x.Id.Equals(request.EventId));

        var imgUrl = "";
        if (request.Image != null)
        {
            string imgName = !existedEvent.CoverImageLink.Equals("") ? existedEvent.CoverImageLink.Split('/')[existedEvent.CoverImageLink.Split('/').Length - 1] : existedEvent.EventTitle + ".png";
            imgUrl = await _azureService.UploadFile(request.Image, imgName);
            existedEvent.CoverImageLink = imgUrl;
        }

        existedEvent.EventTitle = request.EventTitle ?? existedEvent.EventTitle;
        existedEvent.IsPrivate = request.IsPrivate;
        existedEvent.LastUpdatedAt = LocalDateTime.FromDateTime(DateTime.Now);
        _context.Events.Update(existedEvent);
        await _context.SaveChangesAsync(cancellationToken);

        return _mapper.Map<EventDto>(existedEvent);
    }
}