using Application.Common.Interfaces;
using Application.Common.Models.Dtos;

namespace Application.UtilityServices.Commands.UpdateUtilityService;

public record UpdateUtilityServiceCommand : IRequest<UtilityServiceDto>
{
    public Guid Id { get; init; }
    public string Name { get; init; }
    public string ImgUrl { get; init; }
    public double ServiceFee { get; init; }
    public bool IsAllowToRent {  get; init; }
}

public class UpdateUtilityServiceCommandHandler : IRequestHandler<UpdateUtilityServiceCommand, UtilityServiceDto>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;
    
    public UpdateUtilityServiceCommandHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }
    
    public async Task<UtilityServiceDto> Handle(UpdateUtilityServiceCommand request, CancellationToken cancellationToken)
    {
        var entity = await _context.UtilityServices.FirstOrDefaultAsync(x => x.Id == request.Id);
        
        if (entity is null)
        {
            throw new KeyNotFoundException($"Utility service with Id {request.Id} does not exist");
        }
        
        entity.ServiceName = request.Name;
        entity.ServiceImage = request.ImgUrl;
        entity.Fee = request.ServiceFee;
        _context.UtilityServices.Update(entity);
        await _context.SaveChangesAsync(cancellationToken);

        foreach (var workspaceUtility 
                 in await _context.WorkspaceUtilityServices
                     .Where(X => X.Id == entity.Id)
                     .ToListAsync(cancellationToken))
        {
            workspaceUtility.IsAllowToRent = request.IsAllowToRent;
            _context.WorkspaceUtilityServices.Update(workspaceUtility);
            await _context.SaveChangesAsync(cancellationToken);
        }
        
        return _mapper.Map<UtilityServiceDto>(entity);
    }
}