using System.Diagnostics;
using Application.Common.Interfaces;
using Application.Common.Models.Dtos;
using Domain.Entities;
using NodaTime;

namespace Application.Sizes.Commands.AddSize;

public record AddSizeCommand : IRequest<SizeDto>
{
    public string SizeName { get; init; }
    public double PriceAdjustment { get; init; }
    public string[]? Sizes { get; init; } = [];
}

public class AddSizeCommandHandler : IRequestHandler<AddSizeCommand, SizeDto>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;
    
    public AddSizeCommandHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }
    
    public async Task<SizeDto> Handle(AddSizeCommand request, CancellationToken cancellationToken)
    {
        // Credits to TriHTM171368 for patching this code
        try
        {
            dynamic result = "";
            if (request.Sizes?.Length > 0 && _context.Sizes.ToList().Count <= 0)
            {
                for (int i = 0; i < request.Sizes.Length; i++)
                {
                    var size = new Size
                    {
                        SizeName = request.Sizes[i].Split(":")[0],
                        PriceAdjustment = double.Parse(request.Sizes[i].Split(":")[1]),
                        IsActive = true,
                        CreatedAt = LocalDateTime.FromDateTime(DateTime.Now),
                        LastUpdatedAt = LocalDateTime.FromDateTime(DateTime.Now)
                    };
                    result = await _context.Sizes.AddAsync(size);
                }
            }
            else
            {
                var size = new Size
                {
                    SizeName = request.SizeName,
                    PriceAdjustment = request.PriceAdjustment,
                    IsActive = true,
                    CreatedAt = LocalDateTime.FromDateTime(DateTime.Now),
                    LastUpdatedAt = LocalDateTime.FromDateTime(DateTime.Now)
                };
        
                result = await _context.Sizes.AddAsync(size);
            }
            
            await _context.SaveChangesAsync(cancellationToken);
            return _mapper.Map<SizeDto>(result.Entity);
        }
        catch (Exception ex)
        {
            Debug.WriteLine(ex);
            throw new Exception(ex.InnerException.Message);
        }
    }
}