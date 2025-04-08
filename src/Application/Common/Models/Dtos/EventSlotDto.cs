using Application.Common.Mappings;
using Domain.Entities;

namespace Application.Common.Models.Dtos;

public class EventSlotDto : BaseDto, IMapFrom<EventSlot> {
    public Guid SlotId { get; set; }
    public Guid EventId { get; set; }

    public virtual SlotDto Slot { get; set; } = null!;

    public void Mapping(Profile profile) {
        profile.CreateMap<EventSlot, EventSlotDto>();
    }
}
