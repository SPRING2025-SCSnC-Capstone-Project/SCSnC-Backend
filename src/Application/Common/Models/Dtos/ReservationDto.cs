using Application.Common.Mappings;
using Domain.Entities;

namespace Application.Common.Models.Dtos;

public class ReservationDto : BaseDto, IMapFrom<Reservation> {
    public DateOnly ReservationDate { get; set; }
    public WorkspaceDto Workspace { get; set; } = null!;
    public double Deposit { get; set; }
    public TimeOnly StartTime { get; set; }
    public TimeOnly EndTime { get; set; }
    public UserDto User { get; set; } = null!;
    public bool IsFullPaid { get; set; }
    public double TotalPrice { get; set; }

    public void Mapping(Profile profile) {
        profile.CreateMap<Reservation, ReservationDto>()
            .ForMember(dest => dest.ReservationDate, opt => opt.MapFrom(src => src.ReservationDate.ToDateOnly()))
            .ForMember(dest => dest.StartTime, opt => opt.MapFrom(src => src.StartTime.ToTimeOnly()))
            .ForMember(dest => dest.EndTime, opt => opt.MapFrom(src => src.EndTime.ToTimeOnly()));
    }
}
