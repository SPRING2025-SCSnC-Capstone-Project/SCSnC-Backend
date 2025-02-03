using Application.Common.Mappings;
using Domain.Entities;

namespace Application.Common.Models.Dtos;

public class SlotDto : BaseDto, IMapFrom<Slot> {
    public int SlotNumber { get; set; }
    public TimeOnly TimeStart { get; set; }
    public TimeOnly TimeEnd { get; set; }

   public void Mapping(Profile profile) {
        profile.CreateMap<Slot, SlotDto>()
            .ForMember(dest => dest.TimeStart, opt => opt.MapFrom(src => src.TimeStart.ToTimeOnly()))
            .ForMember(dest => dest.TimeEnd, opt => opt.MapFrom(src => src.TimeEnd.ToTimeOnly()));
    }
}