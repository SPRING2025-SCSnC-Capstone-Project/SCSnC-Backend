using Application.Common.Interfaces;
using Application.Common.Models.Dtos;
using Domain.Entities;
using NodaTime;

namespace Application.Sizes.Commands.AddSize;

public record AddSizeCommand : IRequest<SizeDto>
{
    public string SizeName { get; init; }
    public double PriceAdjustment { get; init; }
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
        var size = new Size
        {
            SizeName = request.SizeName,
            PriceAdjustment = request.PriceAdjustment,
            IsActive = true,
            CreatedAt = LocalDateTime.FromDateTime(DateTime.Now),
            LastUpdatedAt = LocalDateTime.FromDateTime(DateTime.Now)
        };
        
        var result = await _context.Sizes.AddAsync(size);
        await _context.SaveChangesAsync(cancellationToken);
        
        return _mapper.Map<SizeDto>(result.Entity);
    }
}