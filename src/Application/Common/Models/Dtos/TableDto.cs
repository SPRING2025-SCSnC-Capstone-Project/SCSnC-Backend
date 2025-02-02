using Application.Common.Mappings;
using Domain.Entities;

namespace Application.Common.Models.Dtos;

public class TableDto : BaseDto, IMapFrom<Table> {
    public int TableNumber { get; set; }
    public int SeatAmount { get; set; }
    public bool IsAvailable { get; set; }
    public bool IsActive { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime LastUpdatedAt { get; set; }
    
    public void Mapping(Profile profile) {
        profile.CreateMap<Table, TableDto>()
            .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(src => src.CreatedAt.ToDateTimeUnspecified()))
            .ForMember(dest => dest.LastUpdatedAt, opt => opt.MapFrom(src => src.LastUpdatedAt.ToDateTimeUnspecified()));
    }
}