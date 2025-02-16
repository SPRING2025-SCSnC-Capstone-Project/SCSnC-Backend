using Application.Common.Interfaces;
using Application.Common.Models.Dtos;
using NodaTime;

namespace Application.Sizes.Commands.UpdateSize;

public record UpdateSizeCommand : IRequest<SizeDto>
{
    public Guid Id { get; init; }
    public string SizeName { get; init; }
    public double PriceAdjustment { get; init; }
    public bool IsActive { get; init; }
}

public class UpdateSizeCommandHandler : IRequestHandler<UpdateSizeCommand, SizeDto>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;
    
    public UpdateSizeCommandHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }
    
    public async Task<SizeDto> Handle(UpdateSizeCommand request, CancellationToken cancellationToken)
    {
        var size = await _context.Sizes.FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);
        
        if (size is null)
        {
            throw new KeyNotFoundException($"Size with id {request.Id} not found");
        }
        
        size.SizeName = request.SizeName;
        size.PriceAdjustment = request.PriceAdjustment;
        size.LastUpdatedAt = LocalDateTime.FromDateTime(DateTime.Now);
        size.IsActive = request.IsActive;
        
        _context.Sizes.Update(size);
        await _context.SaveChangesAsync(cancellationToken);
        
        return _mapper.Map<SizeDto>(size);
    }
}