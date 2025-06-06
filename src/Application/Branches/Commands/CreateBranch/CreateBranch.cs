using Application.Common.Helpers;
using Application.Common.Interfaces;
using Application.Common.Models.Dtos;
using Domain.Entities;
using NodaTime;

namespace Application.Branches.Commands.CreateBranch;

public record CreateBranchCommand : IRequest<BranchDto>
{
    public string Name { get; init; }
    public string Address { get; init; }
    public string PhoneNumber { get; init; }
    public string Email { get; init; }
    public string ImgUrl { get; init; }
    public string Description { get; init; }
}

public class CreateBranchCommandHandler : IRequestHandler<CreateBranchCommand, BranchDto>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;
    private readonly ISecurityService _securityService;

    public CreateBranchCommandHandler(IApplicationDbContext context, IMapper mapper, ISecurityService securityService)
    {
        _context = context;
        _mapper = mapper;
        _securityService = securityService;
    }

    public async Task<BranchDto> Handle(CreateBranchCommand request, CancellationToken cancellationToken)
    {
        List<WorkspaceTypeAtBranch> workspaceTypesAtBranches = new List<WorkspaceTypeAtBranch>();
        var workspaceTypes = _context.WorkspaceTypes.AsQueryable();

        var branch = new Branch
        {
            BranchName = request.Name,
            BranchAddress = request.Address,
            BranchPhone = request.PhoneNumber,
            BranchEmail = request.Email,
            BranchImage = request.ImgUrl,
            BranchDescription = request.Description,
            IsActive = true,
            CreatedAt = LocalDateTime.FromDateTime(DateTime.Now),
            LastUpdatedAt = LocalDateTime.FromDateTime(DateTime.Now)
        };

        var result = await _context.Branches.AddAsync(branch, cancellationToken);
        
        await _context.SaveChangesAsync(cancellationToken);
        
        // Add default prices for all items at the new branch based on the earliest branch
        var getBranchForDefaultPrice = await _context.Branches.OrderBy(x => x.CreatedAt).FirstOrDefaultAsync(cancellationToken);
        foreach (var item in await _context.Items.ToListAsync(cancellationToken))
        {
            var itemPriceAtBranch = new ItemPriceAtBranch
            {
                ItemId = item.Id,
                BranchId = getBranchForDefaultPrice.Id,
                Price = _context.ItemPricesAtBranches.FirstOrDefault(x => x.ItemId == item.Id && x.BranchId == getBranchForDefaultPrice.Id)?.Price ?? 0,
                CreatedAt = LocalDateTime.FromDateTime(DateTime.Now),
                LastUpdatedAt = LocalDateTime.FromDateTime(DateTime.Now)
            };
            
            await _context.ItemPricesAtBranches.AddAsync(itemPriceAtBranch, cancellationToken);
        }

        foreach(var workspaceType in workspaceTypes)
        {
            var workspaceTypeAtBranch = new WorkspaceTypeAtBranch()
            {
                BranchId = result.Entity.Id,
                WorkspaceTypeId = workspaceType.Id,
                CreatedAt = LocalDateTime.FromDateTime(DateTime.Now),
                LastUpdatedAt = LocalDateTime.FromDateTime(DateTime.Now),
                IsAvailable = false
            };
            workspaceTypesAtBranches.Add(workspaceTypeAtBranch);
        }

        var user = new User()
        {
            Username = result.Entity.BranchEmail.Split("@")[0],
            Role = "staff",
            Email = result.Entity.BranchEmail,
            Phone = result.Entity.BranchPhone,
            Address = "",
            FullName = result.Entity.BranchEmail.Split("@")[0],
            IsActive = true,
            CreatedAt = LocalDateTime.FromDateTime(DateTime.Now),
            LastUpdatedAt = LocalDateTime.FromDateTime(DateTime.Now),
            AvatarLink = "",
            AccountType = "manual",
            BranchId = result.Entity.Id
        };

        var salt = StringUtils.RandomString(15);

        var hashedPassword = _securityService.Hash("12345678", salt, result.Entity.BranchEmail);
            var b64HashedPassword = Convert.ToBase64String(hashedPassword);
            user.PasswordHash = $"{salt}{b64HashedPassword}";

        await _context.Users.AddAsync(user, cancellationToken);
        await _context.WorkspaceTypeAtBranches.AddRangeAsync(workspaceTypesAtBranches, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);

        return _mapper.Map<BranchDto>(branch);
    }
}