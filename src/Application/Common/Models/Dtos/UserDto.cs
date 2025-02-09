using Application.Common.Mappings;
using Domain.Entities;

namespace Application.Common.Models.Dtos;

public class UserDto : BaseDto, IMapFrom<User> {
    public string Username { get; set; }
    public string FullName { get; set; }
    public string Email { get; set; }
    public string Phone { get; set; }
    public string Address { get; set; }
    public string Role { get; set; }
    public string? AvatarLink { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime LastUpdatedAt { get; set; }

}