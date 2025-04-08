using Application.Common.Mappings;
using Domain.Entities;

namespace Application.Common.Models.Dtos;

public class ReservationDto : BaseDto, IMapFrom<Reservation> {
    public DateOnly ReserveDate { get; set; }
    public WorkspaceDto Workspace { get; set; } = null!;
    public double Deposit { get; set; }
    public UserDto User { get; set; } = null!;
    public bool IsFullPaid { get; set; }
    public double TotalPrice { get; set; }
    public HashSet<ReservedSlotDto> ReservedSlots { get; set; } = null!;

    public void Mapping(Profile profile) {
        profile.CreateMap<Reservation, ReservationDto>()
            .ForMember(dest => dest.ReserveDate, opt => opt.MapFrom(src => src.ReserveDate.ToDateOnly()));
    }
}
