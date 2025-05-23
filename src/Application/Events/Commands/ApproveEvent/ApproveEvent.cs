using Application.Common.Exceptions;
using Application.Common.Helpers;
using Application.Common.Interfaces;
using Application.Common.Models.Dtos;
using Domain.Entities;
using Microsoft.AspNetCore.Http.HttpResults;
using NodaTime;
using System.Diagnostics;

namespace Application.Reservations.Commands;

public record ApproveEventCommand : IRequest<EventDto>
{
    public Guid EventId { get; set; }
}

public class ApproveEventCommandHandler : IRequestHandler<ApproveEventCommand, EventDto>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;
    private readonly IPaymentService _vnpayService;

    public ApproveEventCommandHandler(IMapper mapper, IApplicationDbContext context, IPaymentService vnpayService)
    {
        _mapper = mapper;
        _context = context;
        _vnpayService = vnpayService;
    }

    public async Task<EventDto> Handle(ApproveEventCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var reservationEvent = await _context.Events.Include(x => x.Reservation).FirstOrDefaultAsync(x => x.Id.Equals(request.EventId));

            reservationEvent.IsActive = true;
            _context.Events.Update(reservationEvent);

            await _context.SaveChangesAsync(cancellationToken);

            var result = _mapper.Map<EventDto>(reservationEvent);
            return result;
        }
        catch (Exception ex)
        {
            Debug.WriteLine(ex);
            throw new Exception(ex.Message, ex);
        }

    }

    private async Task<bool> CheckConflict(DateOnly reservationDate, Guid slotId, CancellationToken cancellationToken)
    {
        var conflict = await _context.ReservedSlots.Include(x => x.Reservation).FirstOrDefaultAsync(x =>
                x.Reservation!.ReserveDate == LocalDate.FromDateOnly(reservationDate) &&
                x.SlotId == slotId, cancellationToken);

        return conflict is not null;
    }

}
