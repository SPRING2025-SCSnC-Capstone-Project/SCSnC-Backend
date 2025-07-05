using Application.Common.Interfaces;
using Application.Common.Models.Dtos;
using NodaTime;

namespace Application.Branches.Commands.DeleteBranch;

public record DeleteBranchCommand : IRequest<BranchDto>
{
    public Guid Id { get; init; }
}

public class DeleteBranchCommandHandler : IRequestHandler<DeleteBranchCommand, BranchDto>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;
    
    public DeleteBranchCommandHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }
    
    public async Task<BranchDto> Handle(DeleteBranchCommand request, CancellationToken cancellationToken)
    {
        var branch = await _context.Branches.FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);
        
        if (branch is null)
        {
            throw new KeyNotFoundException($"Branch with id {request.Id} not found");
        }
        
        branch.IsActive = false;
        branch.LastUpdatedAt = LocalDateTime.FromDateTime(DateTime.Now);
        
        _context.Branches.Update(branch);
        await _context.SaveChangesAsync(cancellationToken);
        
        return _mapper.Map<BranchDto>(branch);
    }
}