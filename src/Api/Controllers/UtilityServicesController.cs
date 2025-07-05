using Api.Controllers.Payload.Requests;
using Api.Controllers.Payload.Requests.UtilityServices;
using Application.Common.Models;
using Application.Common.Models.Dtos;
using Application.UtilityServices.Commands.AddUtilityService;
using Application.UtilityServices.Commands.UpdateUtilityService;
using Application.UtilityServices.Queries.GetUltilityServiceById;
using Application.UtilityServices.Queries.GetUtilityServicesPaginated;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

public class UtilityServicesController: ApiControllerBase
{
    [HttpGet]
    public async Task<ActionResult<Result<PaginatedList<UtilityServiceDto>>>> GetVouchers([FromQuery] PaginatedQueryParameters request)
    {
        var query = new GetUtilityServicesPaginatedQuery()
        {
            Page = request.Page,
            Size = request.Size,
            SortBy = request.SortBy,
            SortOrder = request.SortOrder,
        };

        var result = await Mediator.Send(query);

        return Ok(Result<PaginatedList<UtilityServiceDto>>.Succeed(result));
    }
    
    [HttpGet("{id:guid}")]
    public async Task<ActionResult<Result<UtilityServiceDto>>> GetUtilityServiceById([FromRoute] Guid id)
    {
        var query = new GetUtilityServiceByIdQuery()
        {
            Id = id
        };

        var result = await Mediator.Send(query);

        return Ok(Result<UtilityServiceDto>.Succeed(result));
    }
    
    [HttpPost]
    public async Task<ActionResult<Result<UtilityServiceDto>>> AddUtilityService([FromBody] AddUtilityServiceRequest request)
    {
        var command = new AddUtilityServiceCommand()
        {
            Name = request.ServiceName,
            ImgUrl = request.ServiceImage,
            ServiceFee = request.ServiceFee
        };

        var result = await Mediator.Send(command);

        return CreatedAtAction(nameof(GetUtilityServiceById), new { id = result.Id }, Result<UtilityServiceDto>.Succeed(result));
    }
    
    [HttpPut("{id:guid}")]
    public async Task<ActionResult<Result<UtilityServiceDto>>> UpdateUtilityService([FromRoute] Guid id, [FromBody] UpdateUtilityServiceRequest request)
    {
        var command = new UpdateUtilityServiceCommand()
        {
            Id = id,
            Name = request.ServiceName,
            ImgUrl = request.ServiceImage,
            ServiceFee = request.ServiceFee
        };

        var result = await Mediator.Send(command);

        return Ok(Result<UtilityServiceDto>.Succeed(result));
    }
}