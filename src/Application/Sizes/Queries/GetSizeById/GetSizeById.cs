using Application.Common.Interfaces;
using Application.Common.Models.Dtos;

namespace Application.Sizes.Queries.GetSizeById;

public record GetSizeByIdQuery : IRequest<SizeDto>
{
    public Guid Id { get; init; }
}

public class GetSizeByIdQueryHandler : IRequestHandler<GetSizeByIdQuery, SizeDto>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;
    
    public GetSizeByIdQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }
    
    public async Task<SizeDto> Handle(GetSizeByIdQuery request, CancellationToken cancellationToken)
    {
        var size = await _context.Sizes.FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);
        
        if (size is null)
        {
            throw new KeyNotFoundException($"Size with id {request.Id} not found");
        }
        
        return _mapper.Map<SizeDto>(size);
    }
}