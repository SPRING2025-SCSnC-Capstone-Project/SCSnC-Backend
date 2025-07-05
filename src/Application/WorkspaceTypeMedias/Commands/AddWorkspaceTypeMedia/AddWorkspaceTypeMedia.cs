using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Application.Common.Models.Dtos;
using Domain.Entities;
using Microsoft.AspNetCore.Mvc.Formatters;
using System.Diagnostics;

namespace Application.WorkspaceTypes.Commands;

public record AddWorkspaceTypeMediaCommand : IRequest<WorkspaceMediaDto>
{
}

public class AddWorkspaceTypeMediaCommandHandler : IRequestHandler<AddWorkspaceTypeMediaCommand, WorkspaceMediaDto>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public AddWorkspaceTypeMediaCommandHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<WorkspaceMediaDto> Handle(AddWorkspaceTypeMediaCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var workspaceTypes = _context.WorkspaceTypes.OrderBy(x => x.PricePerHour).ToList();
            dynamic result = "";
            List<List<WorkspaceMediaDto>> workspaceMedias = new List<List<WorkspaceMediaDto>>()
        {
            new List<WorkspaceMediaDto>()
            {
                new WorkspaceMediaDto()
                {
                    MediaType = "3d model",
                    MediaUrl = "https://workspacemodel.blob.core.windows.net/3dmodels/singleroom1.glb"
                },
                new WorkspaceMediaDto()
                {
                    MediaType = "image",
                    MediaUrl = "https://scsnc0images.blob.core.windows.net/images/e752eb1a13e4ea9c32661cf46f1b4209073aa29d.jpg"
                },
                new WorkspaceMediaDto()
                {
                    MediaType = "image",
                    MediaUrl = "https://scontent.fsgn2-9.fna.fbcdn.net/v/t1.15752-9/476498801_1208503627473892_7005872065053806864_n.png?_nc_cat=106&ccb=1-7&_nc_sid=0024fc&_nc_ohc=EEslPM92vsYQ7kNvwE0_30u&_nc_oc=Adks9QSzpx0-hWJ6_tlOXkHAjt_nCfzYQ97L1EiFcJCffw1Nq28nvembnA97P75va28&_nc_ad=z-m&_nc_cid=0&_nc_zt=23&_nc_ht=scontent.fsgn2-9.fna&oh=03_Q7cD2QFhO6FJVdXbwEKF-w5uWHBexjqRcdR4cQ7XM6M54rjJNQ&oe=6848EE33"
                },
                new WorkspaceMediaDto()
                {
                    MediaType = "image",
                    MediaUrl = "https://scontent.fsgn2-9.fna.fbcdn.net/v/t1.15752-9/480786380_1011620507691217_7826549219124420975_n.png?_nc_cat=106&ccb=1-7&_nc_sid=0024fc&_nc_ohc=hUabJJKBsAQQ7kNvwGI6LoF&_nc_oc=Adm02v1MNBt62nO_3Iqn-9wLAb6fiG8Zk7cicvOQFE-LYQzHFFJLrAmjpKIliG928zs&_nc_ad=z-m&_nc_cid=0&_nc_zt=23&_nc_ht=scontent.fsgn2-9.fna&oh=03_Q7cD2QH8WBvS6jF1_e_DNbizJufhlhXBJQwLK6OX4xGBsu_Frg&oe=6848FC63"
                },
            },
            new List<WorkspaceMediaDto>()
            {
                new WorkspaceMediaDto()
                {
                    MediaType = "3d model",
                    MediaUrl = "https://workspacemodel.blob.core.windows.net/3dmodels/coupleroom.glb"
                },
                new WorkspaceMediaDto()
                {
                    MediaType = "image",
                    MediaUrl = "https://scsnc0images.blob.core.windows.net/images/6bbdbe046951c1271eb72034632bf6fced741d8d.png"
                },
                new WorkspaceMediaDto()
                {
                    MediaType = "image",
                    MediaUrl = "https://scsnc0images.blob.core.windows.net/images/6bbdbe046951c1271eb72034632bf6fced741d8d.png"
                },
                new WorkspaceMediaDto()
                {
                    MediaType = "image",
                    MediaUrl = "https://scsnc0images.blob.core.windows.net/images/6bbdbe046951c1271eb72034632bf6fced741d8d.png"
                },
            },
            new List<WorkspaceMediaDto>()
            {
                new WorkspaceMediaDto()
                {
                    MediaType = "3d model",
                    MediaUrl = "https://workspacemodel.blob.core.windows.net/3dmodels/tearoom.glb"
                },
                new WorkspaceMediaDto()
                {
                    MediaType = "image",
                    MediaUrl = "https://scsnc0images.blob.core.windows.net/images/ce19db6291c8b722f07e2cdfafc90e68efb0910b.png"
                },
                new WorkspaceMediaDto()
                {
                    MediaType = "image",
                    MediaUrl = "https://scsnc0images.blob.core.windows.net/images/ce19db6291c8b722f07e2cdfafc90e68efb0910b.png"
                },
                new WorkspaceMediaDto()
                {
                    MediaType = "image",
                    MediaUrl = "https://scsnc0images.blob.core.windows.net/images/ce19db6291c8b722f07e2cdfafc90e68efb0910b.png"
                },
            },
            new List<WorkspaceMediaDto>()
            {
                new WorkspaceMediaDto()
                {
                    MediaType = "3d model",
                    MediaUrl = "https://workspacemodel.blob.core.windows.net/3dmodels/meetingroom.glb"
                },
                new WorkspaceMediaDto()
                {
                    MediaType = "image",
                    MediaUrl = "https://scsnc0images.blob.core.windows.net/images/024c75d3294d305df1e4da710a7c03d7ae24422b.png"
                },
                new WorkspaceMediaDto()
                {
                    MediaType = "image",
                    MediaUrl = "https://scsnc0images.blob.core.windows.net/images/024c75d3294d305df1e4da710a7c03d7ae24422b.png"
                },
                new WorkspaceMediaDto()
                {
                    MediaType = "image",
                    MediaUrl = "https://scsnc0images.blob.core.windows.net/images/024c75d3294d305df1e4da710a7c03d7ae24422b.png"
                },
            }
        };
            for (int i = 0; i < workspaceTypes.Count; i++)
            {
                for (int z = 0; z < workspaceMedias[i].Count; z++)
                {
                    var entity = new WorkspaceMedia()
                    {
                        WorkspaceTypeId = workspaceTypes[i].Id,
                        MediaType = workspaceMedias[i][z].MediaType,
                        MediaUrl = workspaceMedias[i][z].MediaUrl
                    };
                    result = await _context.WorkspaceMedias.AddAsync(entity, cancellationToken);
                }
            }
            await _context.SaveChangesAsync(cancellationToken);

            return _mapper.Map<WorkspaceMediaDto>(result);
        }
        catch (Exception e)
        {
            Debug.WriteLine(e);
            throw new Exception(e.Message, e);
        }

    }
}