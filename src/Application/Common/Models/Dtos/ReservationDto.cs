using Application.Common.Mappings;
using Domain.Entities;
using Microsoft.AspNetCore.Http;
using NodaTime;

namespace Application.Common.Models.Dtos;

public class ReservationDto : BaseDto, IMapFrom<Reservation>
{
    public DateOnly ReserveDate { get; set; }
    public WorkspaceDto Workspace { get; set; } = null!;
    public double Deposit { get; set; }
    public UserDto? User { get; set; } = null!;
    public bool IsFullPaid { get; set; }
    public double TotalPrice { get; set; }
    public string Status { get; set; } = null!;
    public HashSet<ReservedSlotDto> ReservedSlots { get; set; } = null!;
    public string? Note { get; set; }
    public string Email { get; set; }
    public string? Phone { get; set; }
    //public DateTimeOffset StartDate { get; set; }
    //public DateTimeOffset EndDate { get; set; }
    public string PaymentLink { get; set; }
    public Event? Event { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime LastUpdatedAt { get; set; }
    public TimeOnly TimeStart { get; set; }
    public TimeOnly TimeEnd { get; set; }
    public bool IsCanceled { get; set; }
    public HashSet<TransactionDto> Transactions { get; set; } = null!;


    public void Mapping(Profile profile)
    {
        profile.CreateMap<Reservation, ReservationDto>()
            .ForMember(dest => dest.ReserveDate, opt => opt.MapFrom(src => src.ReserveDate.ToDateOnly()))
            .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(src => src.CreatedAt.ToDateTimeUnspecified()))
            .ForMember(dest => dest.LastUpdatedAt, opt => opt.MapFrom(src => src.LastUpdatedAt.ToDateTimeUnspecified()))
            .ForMember(dest => dest.TimeStart, opt => opt.MapFrom(src => src.TimeStart.Value.ToTimeOnly()))
            .ForMember(dest => dest.TimeEnd, opt => opt.MapFrom(src => src.TimeEnd.Value.ToTimeOnly()));
    }
}

public class ResponseReservationDto : BaseDto, IMapFrom<Reservation>
{
    public DateOnly ReserveDate { get; set; }
    public WorkspaceDto Workspace { get; set; } = null!;
    public double Deposit { get; set; }
    public UserDto? User { get; set; } = null!;
    public bool IsFullPaid { get; set; }
    public bool IsCanceled { get; set; }
    public double TotalPrice { get; set; }
    public string Status { get; set; } = null!;
    public string PaymentLink { get; set; }
    public HashSet<ReservedSlotDto> ReservedSlots { get; set; } = null!;
    public string? Note { get; set; }
    public string Email { get; set; }
    public string? Phone { get; set; }
    //public DateTimeOffset StartDate { get; set; }
    //public DateTimeOffset EndDate { get; set; }
    public Event? Event { get; set; }
    public BranchDto Branch { get; set; }
    public TimeOnly TimeStart { get; set; }
    public TimeOnly TimeEnd { get; set; }

    public void Mapping(Profile profile)
    {
        profile.CreateMap<Reservation, ResponseReservationDto>()
            .ForMember(dest => dest.ReserveDate, opt => opt.MapFrom(src => src.ReserveDate.ToDateOnly()))
            .ForMember(dest => dest.TimeStart, opt => opt.MapFrom(src => src.TimeStart.Value.ToTimeOnly()))
            .ForMember(dest => dest.TimeEnd, opt => opt.MapFrom(src => src.TimeEnd.Value.ToTimeOnly()));
    }
}
