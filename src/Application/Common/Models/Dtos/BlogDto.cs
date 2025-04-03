using Application.Common.Mappings;
using Domain.Entities;

namespace Application.Common.Models.Dtos;

public class BlogDto: BaseDto, IMapFrom<Blog>
{
    public string EventName { get; set; }
    public string UploadedBy { get; set; }
    public string Title { get; set; }
    public string Content { get; set; }
    public List<BlogMediaDto> Media { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime LastUpdatedAt { get; set; }
    
    public void Mapping(Profile profile)
    {
        profile.CreateMap<Blog, BlogDto>()
            .ForMember(d => d.EventName, opt => opt.MapFrom(s => s.Event.EventTitle))
            .ForMember(d => d.UploadedBy, opt => opt.MapFrom(s => s.User.FullName))
            .ForMember(d => d.Media, opt => opt.MapFrom(s => s.BlogMedias))
            .ForMember(d => d.CreatedAt, opt => opt.MapFrom(s => s.CreatedAt))
            .ForMember(d => d.LastUpdatedAt, opt => opt.MapFrom(s => s.LastUpdatedAt));
    }
}