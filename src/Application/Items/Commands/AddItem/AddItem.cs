using System.Diagnostics;
using Application.Common.Interfaces;
using Application.Common.Models.Dtos;
using Application.ItemWithSizes.Commands.CreateItemWithSize;
using Domain.Entities;
using NodaTime;
using System.Diagnostics;
using Microsoft.AspNetCore.Http;
using System.Text.RegularExpressions;

namespace Application.Items.Commands.AddItem;

public record AddItemCommand : IRequest<ItemDto>
{
    public string Name { get; init; }
    public string Description { get; init; }
    public IFormFile? Img { get; set; }
    public Guid CategoryId { get; init; }
    public List<Guid> SizeIds { get; init; }
    public bool? AutoCreate { get; init; } = false;
    public Dictionary<Guid, int> BranchPrices { get; set; }
}

public class AddItemCommandHandler : IRequestHandler<AddItemCommand, ItemDto>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;
    private readonly IAzureService _azureService;

    public AddItemCommandHandler(IApplicationDbContext context, IMapper mapper, IAzureService azureService)
    {
        _context = context;
        _mapper = mapper;
        _azureService = azureService;
    }

    public async Task<ItemDto> Handle(AddItemCommand request, CancellationToken cancellationToken)
    {
        // Credits to TriHTM171368 for patching this code
        try
        {
            double basePrice = 65.000;
            double[] priceArr = [35.000, -15.000, -1.000];
            List<ItemCategory> categories = _context.ItemCategories.ToList();
            List<Size> sizes = _context.Sizes.ToList();
            List<Item> items = new List<Item>();
            List<Branch> branches = _context.Branches.ToList();
            if (request.AutoCreate.Value)
            {
                for (int i = 0; i < categories.Count; i++)
                {
                    for (int j = 0; j < 10; j++)
                    {
                        var entity = new Item
                        {
                            ItemName = categories[i].CategoryName + " " + (j + 1),
                            ItemDescription = categories[i].CategoryName + " " + (j + 1),
                            //ItemBasePrice = 65.000,
                            ItemCategoryId = categories[i].Id,
                            ItemImg = "",
                            IsActive = true,
                            CreatedAt = LocalDateTime.FromDateTime(DateTime.Now),
                            LastUpdatedAt = LocalDateTime.FromDateTime(DateTime.Now)
                        };

                        var result = await _context.Items.AddAsync(entity, cancellationToken);
                        items.Add(result.Entity);
                    }
                }
                for (int z = 0; z < items.Count; z++)
                {
                    foreach (var size in sizes)
                    {
                        var itemWithSize = new ItemWithSize
                        {
                            ItemId = items[z].Id,
                            SizeId = size.Id,
                            IsActive = true
                        };

                        await _context.ItemWithSizes.AddAsync(itemWithSize, cancellationToken);
                    }
                }

                for(int i = 0; i < branches.Count; i++)
                {
                    for (int j = 0; j < items.Count; j++)
                    {
                        var itemPriceAtBranch = new ItemPriceAtBranch
                        {
                            ItemId = items[j].Id,
                            BranchId = branches[i].Id,
                            Price = basePrice + priceArr[i],
                            CreatedAt = LocalDateTime.FromDateTime(DateTime.Now),
                            LastUpdatedAt = LocalDateTime.FromDateTime(DateTime.Now)
                        };
                        await _context.ItemPricesAtBranches.AddAsync(itemPriceAtBranch, cancellationToken);
                    }
                }

                await _context.SaveChangesAsync(cancellationToken);
                return _mapper.Map<ItemDto>(items[items.Count - 1]);

            }
            else
            {
                var imgUrl = "";
                if(request.Img != null)
                {
                    imgUrl = await _azureService.UploadFile(request.Img, $"{Regex.Replace(request.Name, @"\s", string.Empty) + Guid.NewGuid()}.png");
                }
                var entity = new Item
                {
                    ItemName = request.Name,
                    ItemDescription = request.Description,
                    ItemCategoryId =request.CategoryId,
                    ItemImg = imgUrl,
                    IsActive = true,
                    CreatedAt = LocalDateTime.FromDateTime(DateTime.Now),
                    LastUpdatedAt = LocalDateTime.FromDateTime(DateTime.Now)
                };

                var result = await _context.Items.AddAsync(entity, cancellationToken);
        
                await _context.SaveChangesAsync(cancellationToken); 
                
//              Note: add to ItemWithSize table when adding new item
                foreach (var size in sizes)
                {
                    var itemWithSize = new ItemWithSize
                    {
                        ItemId = result.Entity.Id,
                        SizeId = size.Id,
                        IsActive = false
                    };
            
                    await _context.ItemWithSizes.AddAsync(itemWithSize, cancellationToken);
                } 
//              Note: end
                await _context.SaveChangesAsync(cancellationToken);
                
//              Note: add default price to ItemPriceAtBranch table when adding new item
                foreach (var branch in request.BranchPrices)
                {
                    var itemPriceAtBranch = new ItemPriceAtBranch
                    {
                        ItemId = result.Entity.Id,
                        BranchId = branch.Key,
                        Price = branch.Value,
                        CreatedAt = LocalDateTime.FromDateTime(DateTime.Now),
                        LastUpdatedAt = LocalDateTime.FromDateTime(DateTime.Now)
                    };
                    
                    await _context.ItemPricesAtBranches.AddAsync(itemPriceAtBranch, cancellationToken);
                }
                await _context.SaveChangesAsync(cancellationToken);
                
                return _mapper.Map<ItemDto>(result.Entity);
            }
        }
        catch (Exception ex)
        {
            Debug.WriteLine(ex);
            throw new Exception(ex.InnerException.Message);
        }
    }
}