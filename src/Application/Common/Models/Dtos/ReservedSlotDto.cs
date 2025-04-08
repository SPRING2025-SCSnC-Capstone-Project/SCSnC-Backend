using Application.Common.Mappings;
using Domain.Entities;

namespace Application.Common.Models.Dtos;

public class ReservedSlotDto : BaseDto, IMapFrom<ReservedSlot> {
    public Guid SlotId { get; set; }
    public Guid ReservationId { get; set; }

    public virtual SlotDto Slot { get; set; } = null!;

    public void Mapping(Profile profile) {
        profile.CreateMap<ReservedSlot, ReservedSlotDto>();
    }
}
