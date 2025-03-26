using Application.Common.Interfaces;
using Application.Common.Models.Dtos;
using Domain.Entities;
using NodaTime;
using System.Diagnostics;

namespace Application.ItemCategories.Commands.CreateItemCategory;

public record CreateItemCategoryCommand: IRequest<ItemCategoryDto>
{
    public string Name { get; init; }
    public string[]? Catagories { get; init; } = [];
}

public class CreateItemCategoryCommandHandler : IRequestHandler<CreateItemCategoryCommand, ItemCategoryDto>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;
    
    public CreateItemCategoryCommandHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }
    
    public async Task<ItemCategoryDto> Handle(CreateItemCategoryCommand request, CancellationToken cancellationToken)
    {

        try
        {
            dynamic res = "";
            if (request.Catagories?.Length > 0 && _context.ItemCategories.ToList().Count <= 0)
            {
                for (int i = 0; i < request.Catagories.Length; i++)
                {
                    var itemCategory = new ItemCategory
                    {
                        CategoryName = request.Catagories[i],
                        IsActive = true,
                        CreatedAt = LocalDateTime.FromDateTime(DateTime.Now),
                        LastUpdatedAt = LocalDateTime.FromDateTime(DateTime.Now)
                    };
                    res = await _context.ItemCategories.AddAsync(itemCategory, cancellationToken);
                }
            }
            else
            {
                var itemCategory = new ItemCategory
                {
                    CategoryName = request.Name,
                    IsActive = true,
                    CreatedAt = LocalDateTime.FromDateTime(DateTime.Now),
                    LastUpdatedAt = LocalDateTime.FromDateTime(DateTime.Now)
                };

                res = await _context.ItemCategories.AddAsync(itemCategory, cancellationToken);
            }

            await _context.SaveChangesAsync(cancellationToken);
            return _mapper.Map<ItemCategoryDto>(res.Entity);

        }
        catch (Exception ex)
        {
            Debug.WriteLine(ex);
            throw new Exception(ex.InnerException.Message);
        }
    }
}