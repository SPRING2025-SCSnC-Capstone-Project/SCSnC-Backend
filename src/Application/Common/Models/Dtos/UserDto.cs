using Application.Common.Mappings;
using Domain.Entities;

namespace Application.Common.Models.Dtos;

public class UserDto : BaseDto, IMapFrom<User> {
    public string AccountType { get; set; } = null!;
    public string Username { get; set; } = null!;
    public string? FullName { get; set; }
    public string Email { get; set; } = null!;
    public string? Phone { get; set; }
    public string? Address { get; set; }
    public string Role { get; set; } = null!;
    public string? AvatarLink { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime LastUpdatedAt { get; set; }

    public void Mapping(Profile profile) {
        profile.CreateMap<User, UserDto>()
            .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(src => src.CreatedAt.ToDateTimeUnspecified()))
            .ForMember(dest => dest.LastUpdatedAt, opt => opt.MapFrom(src => src.LastUpdatedAt.ToDateTimeUnspecified()));
            
    }
}
