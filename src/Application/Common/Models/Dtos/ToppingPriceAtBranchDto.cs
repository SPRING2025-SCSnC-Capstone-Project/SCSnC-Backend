using Application.Common.Mappings;
using Domain.Entities;

namespace Application.Common.Models.Dtos;

public class ToppingPriceAtBranchDto: BaseDto, IMapFrom<ToppingPriceAtBranch>
{
    public string BranchName { get; set; }
    public ToppingDto Topping { get; set; }
    public double ToppingPrice { get; set; }
    
    public void Mapping(Profile profile) 
    {
        profile.CreateMap<ToppingPriceAtBranch, ToppingPriceAtBranchDto>()
            .ForMember(dest => dest.BranchName, opt => opt.MapFrom(src => src.Branch.BranchName))
            .ForMember(dest => dest.Topping, opt => opt.MapFrom(src => src.Topping))
            .ForMember(dest => dest.ToppingPrice, opt => opt.MapFrom(src => src.ToppingPrice))
            .ReverseMap();
    }
}

public class ToppingPriceAtAllBranchesDto : BaseDto, IMapFrom<ToppingPriceAtBranch>
{
    public string ToppingName { get; set; }
    public List<CustomDto> Branches { get; set; }
    
    public void Mapping(Profile profile) 
    {
        profile.CreateMap<ToppingPriceAtBranch, ToppingPriceAtAllBranchesDto>()
            .ForMember(dest => dest.ToppingName, opt => opt.MapFrom(src => src.Topping.ToppingName))
            .ForMember(dest => dest.Branches, opt => opt.MapFrom(src => src.Topping.ToppingPricesAtBranches.Select(t => new CustomDto
            {
                BranchName = t.Branch.BranchName,
                ItemPrice = t.ToppingPrice
            }).ToList()))
            .ReverseMap();
    }
}