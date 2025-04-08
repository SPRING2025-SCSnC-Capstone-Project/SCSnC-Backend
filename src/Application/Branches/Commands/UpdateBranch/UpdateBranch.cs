using Application.Common.Interfaces;
using Application.Common.Models.Dtos;
using NodaTime;

namespace Application.Branches.Commands.UpdateBranch;

public record UpdateBranchCommand : IRequest<BranchDto>
{
    public Guid Id { get; init; }
    public string Name { get; init; }
    public string Address { get; init; }
    public string Phone { get; init; }
    public string Description { get; init; }
    public string ImgUrl { get; init; }
    public string Email { get; init; }
}

public class UpdateBranchCommandHandler : IRequestHandler<UpdateBranchCommand, BranchDto>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;
    
    public UpdateBranchCommandHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }
    
    public async Task<BranchDto> Handle(UpdateBranchCommand request, CancellationToken cancellationToken)
    {
        var branch = await _context.Branches.FirstOrDefaultAsync(x => x.Id == request.Id);
        
        if (branch is null)
        {
            throw new KeyNotFoundException($"Branch with id {request.Id} not found");
        }
        
        branch.BranchName = request.Name;
        branch.BranchAddress = request.Address;
        branch.BranchPhone = request.Phone;
        branch.BranchDescription = request.Description;
        branch.BranchImage = request.ImgUrl;
        branch.BranchEmail = request.Email;
        branch.LastUpdatedAt = LocalDateTime.FromDateTime(DateTime.Now);
        
        _context.Branches.Update(branch);
        await _context.SaveChangesAsync(cancellationToken);
        
        return _mapper.Map<BranchDto>(branch);
    }
}