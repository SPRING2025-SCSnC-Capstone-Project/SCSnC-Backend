using Application.Common.Mappings;
using Domain.Entities;

namespace Application.Common.Models.Dtos;

public class EventDto : BaseDto, IMapFrom<Event> {
    public string EventTitle { get; set; } = null!;
    public string EventDescription { get; set; } = null!;
    public DateOnly EventDate { get; set; }
    public string? CoverImageLink { get; set; }
    public double EntranceFee { get; set; }
    public ReservationDto Reservation { get; set; } = null!;
    public DateTime CreatedAt { get; set; }
    public DateTime LastUpdatedAt { get; set; }
    public HashSet<EventSlotDto> EventSlots { get; set; } = null!;

    public void Mapping(Profile profile) {
        profile.CreateMap<Event, EventDto>()
            .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(src => src.CreatedAt.ToDateTimeUnspecified()))
            .ForMember(dest => dest.LastUpdatedAt, opt => opt.MapFrom(src => src.LastUpdatedAt.ToDateTimeUnspecified()))
            .ForMember(dest => dest.EventDate, opt => opt.MapFrom(src => src.EventDate.ToDateOnly()));
    } 
}
