using Application.Common.Interfaces;
using Application.Common.Models.Dtos;
using Domain.Entities;
using Microsoft.AspNetCore.Mvc;

namespace Application.UtilityServices.Commands.AddUtilityService;

public record AddUtilityServiceCommand : IRequest<UtilityServiceDto>
{
    public string Name { get; init; }
    public string ImgUrl { get; init; }
    public double ServiceFee { get; init; }
}

public class AddUtilityServiceCommandHandler : IRequestHandler<AddUtilityServiceCommand, UtilityServiceDto>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public AddUtilityServiceCommandHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }
    
    public async Task<UtilityServiceDto> Handle(AddUtilityServiceCommand request, CancellationToken cancellationToken)
    {
        var utilityService = new UtilityService
        {
            ServiceName = request.Name,
            ServiceImage = request.ImgUrl,
            Fee = request.ServiceFee,
        };
        
        await _context.UtilityServices.AddAsync(utilityService, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);

        foreach (var workspaceType in await _context.WorkspaceTypes.ToListAsync(cancellationToken))
        {
            var workspaceUtility = new WorkspaceUtilityService
            {
                WorkspaceTypeId = workspaceType.Id,
                UtilityServiceId = utilityService.Id,
            };
            await _context.WorkspaceUtilityServices.AddAsync(workspaceUtility, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);
        }
        
        return _mapper.Map<UtilityServiceDto>(utilityService);
    }
}