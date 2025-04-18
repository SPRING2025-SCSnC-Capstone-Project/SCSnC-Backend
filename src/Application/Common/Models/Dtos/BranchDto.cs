using Application.Common.Mappings;
using Domain.Entities;
using NodaTime;

namespace Application.Common.Models.Dtos;

public class BranchDto: BaseDto, IMapFrom<Branch>
{
    public string BranchName { get; set; }
    public string BranchAddress { get; set; }
    public string BranchPhone { get; set; }
    public string BranchEmail { get; set; }
    public string BranchDescription { get; set; }
    public string BranchImage { get; set; }
    public bool IsActive { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime LastUpdatedAt { get; set; }
    
    public void Mapping(AutoMapper.Profile profile)
    {
        profile.CreateMap<Branch, BranchDto>()
            .ForMember(d => d.CreatedAt, opt => opt.MapFrom(s => s.CreatedAt.ToDateTimeUnspecified()))
            .ForMember(d => d.LastUpdatedAt, opt => opt.MapFrom(s => s.LastUpdatedAt.ToDateTimeUnspecified()));
    }
}