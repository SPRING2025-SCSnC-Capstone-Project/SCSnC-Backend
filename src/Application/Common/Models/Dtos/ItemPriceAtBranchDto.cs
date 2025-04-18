using Application.Common.Mappings;
using Domain.Entities;

namespace Application.Common.Models.Dtos;

public class ItemPriceAtBranchDto: BaseDto, IMapFrom<ItemPriceAtBranch>
{
    public string BranchName { get; set; }
    public ItemDto Item { get; set; }
    public double ItemPrice { get; set; }
    
    public void Mapping(AutoMapper.Profile profile)
    {
        profile.CreateMap<ItemPriceAtBranch, ItemPriceAtBranchDto>()
            .ForMember(d => d.BranchName, opt => opt.MapFrom(s => s.Branch.BranchName))
            .ForMember(d => d.Item, opt => opt.MapFrom(s => s.Item))
            .ForMember(d => d.ItemPrice, opt => opt.MapFrom(s => s.Price));
    }
}

public class ItemPriceAtAllBranchesDto : BaseDto, IMapFrom<ItemPriceAtBranch>
{
    public string ItemName { get; set; }
    public List<CustomDto> Branches { get; set; }
    
    public void Mapping(AutoMapper.Profile profile)
    {
        profile.CreateMap<ItemPriceAtBranch, ItemPriceAtAllBranchesDto>()
            .ForMember(d => d.ItemName, opt => opt.MapFrom(s => s.Item.ItemName))
            .ForMember(d => d.Branches, opt => opt.MapFrom(s => s.Item.ItemPricesAtBranches.Select(i => new CustomDto
            {
                BranchName = i.Branch.BranchName,
                ItemPrice = i.Price
            }).ToList()));
    }
}

public class CustomDto
{
    public string BranchName { get; set; }
    public double ItemPrice { get; set; }
}