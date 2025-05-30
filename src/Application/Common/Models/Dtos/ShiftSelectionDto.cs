using Application.Common.Mappings;
using Domain.Entities;

namespace Application.Common.Models.Dtos;

public class ShiftSelectionDto: BaseDto, IMapFrom<ShiftSelection>
{
    public Guid EmployeeId { get; set; }
    public Guid BranchId { get; set; }
    public DateOnly WeekStart { get; set; }
    public DateOnly Date { get; set; }
    public string ShiftTypeName { get; set; }
    public string Status { get; set; }
    
    public void Mapping(Profile profile)
    {
        profile.CreateMap<ShiftSelection, ShiftSelectionDto>()
            .ForMember(dest => dest.WeekStart, opt => opt.MapFrom(src => src.WeekStart.ToDateOnly()))
            .ForMember(dest => dest.Date, opt => opt.MapFrom(src => src.Date.ToDateOnly()))
            .ForMember(dest => dest.ShiftTypeName, opt => opt.MapFrom(src => src.ShiftType.Name));
    }
}

public class DeleteShiftSelectionResponse
{
    public string Message { get; set; }
}