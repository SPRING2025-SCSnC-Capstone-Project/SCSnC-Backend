using Application.Common.Mappings;
using Domain.Entities;

namespace Application.Common.Models.Dtos;

public class BlogMediaDto : BaseDto, IMapFrom<BlogMedia>
{
    public string BlogTitle { get; set; }
    public string MediaType { get; set; }
    public string MediaUrl { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime LastUpdatedAt { get; set; }
    
    public void Mapping(Profile profile)
    {
        profile.CreateMap<BlogMedia, BlogMediaDto>()
            .ForMember(d => d.BlogTitle, opt => opt.MapFrom(s => s.Blog.Title))
            .ForMember(d => d.MediaType, opt => opt.MapFrom(s => s.MediaType))
            .ForMember(d => d.MediaUrl, opt => opt.MapFrom(s => s.MediaUrl))
            .ForMember(d => d.CreatedAt, opt => opt.MapFrom(s => s.CreatedAt))
            .ForMember(d => d.LastUpdatedAt, opt => opt.MapFrom(s => s.LastUpdatedAt));
    }
}

public class AddBlogMediaDto
{
    public string MediaType { get; set; }
    public string MediaUrl { get; set; }
}