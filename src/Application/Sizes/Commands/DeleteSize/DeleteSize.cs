using Application.Common.Interfaces;
using Application.Common.Models.Dtos;

namespace Application.Sizes.Commands.DeleteSize;

public record DeleteSizeCommand : IRequest<SizeDto>
{
    public Guid Id { get; init; }
}

public class DeleteSizeCommandHandler : IRequestHandler<DeleteSizeCommand, SizeDto>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;
    
    public DeleteSizeCommandHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }
    
    public async Task<SizeDto> Handle(DeleteSizeCommand request, CancellationToken cancellationToken)
    {
        var size = await _context.Sizes.FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);
        
        if (size is null)
        {
            throw new KeyNotFoundException($"Size with id {request.Id} not found");
        }
        
        _context.Sizes.Remove(size);
        
        await _context.SaveChangesAsync(cancellationToken);
        
        return _mapper.Map<SizeDto>(size);
    }
}