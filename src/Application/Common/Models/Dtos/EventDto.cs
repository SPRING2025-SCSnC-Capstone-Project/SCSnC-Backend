using Application.Common.Mappings;
using Domain.Entities;

namespace Application.Common.Models.Dtos;

public class EventDto : BaseDto, IMapFrom<Event> {
    public string EventTitle { get; set; }
    public string EventDescription { get; set; }
    public string CoverImageLink { get; set; }
    public double EntranceFee { get; set; }
    public int NumberOfPeople { get; set; }
    public double EventFee { get; set; }
    public DateTime EventStartDate { get; set; }
    public DateTime EventEndDate { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime LastUpdatedAt { get; set; }
    public string PaymentLink { get; set; }

    public void Mapping(Profile profile) {
        profile.CreateMap<Event, EventDto>()
            .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(src => src.CreatedAt.ToDateTimeUnspecified()))
            .ForMember(dest => dest.LastUpdatedAt, opt => opt.MapFrom(src => src.LastUpdatedAt.ToDateTimeUnspecified()))
            .ForMember(dest => dest.EventStartDate, opt => opt.MapFrom(src => src.EventStartDate.ToDateTimeUnspecified()))
            .ForMember(dest => dest.EventEndDate, opt => opt.MapFrom(src => src.EventEndDate.ToDateTimeUnspecified()));
    } 
}
