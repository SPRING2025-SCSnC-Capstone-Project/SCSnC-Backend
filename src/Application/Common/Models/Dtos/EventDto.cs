using Application.Common.Mappings;
using Domain.Entities;

namespace Application.Common.Models.Dtos;

public class EventDto : BaseDto, IMapFrom<Event> {
    public string EventTitle { get; set; } = null!;
    public string EventDescription { get; set; } = null!;
    public string? CoverImageLink { get; set; }
    public double EntranceFee { get; set; }
    public TimeOnly EventStartTime { get; set; }
    public TimeOnly EventEndTime { get; set; }
    public ReservationDto Reservation { get; set; } = null!;
    public DateTime CreatedAt { get; set; }
    public DateTime LastUpdatedAt { get; set; }
    public string PaymentLink { get; set; }

    public void Mapping(Profile profile) {
        profile.CreateMap<Event, EventDto>()
            .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(src => src.CreatedAt.ToDateTimeUnspecified()))
            .ForMember(dest => dest.LastUpdatedAt, opt => opt.MapFrom(src => src.LastUpdatedAt.ToDateTimeUnspecified()))
            .ForMember(dest => dest.EventStartTime, opt => opt.MapFrom(src => src.EventStartTime.ToTimeOnly()))
            .ForMember(dest => dest.EventEndTime, opt => opt.MapFrom(src => src.EventEndTime.ToTimeOnly()));
    } 
}
