using Api.Controllers.Payload.Requests;
using Api.Controllers.Payload.Requests.Vouchers;
using Application.Common.Models;
using Application.Common.Models.Dtos;
using Application.Toppings.Queries.GetToppingsPaginated;
using Application.UserVouchers.Queries.GetUserVoucherById;
using Application.UserVouchers.Queries.GetUserVouchersPaginated;
using Application.Vouchers.Commands.CreateVoucher;
using Application.Vouchers.Commands.DeleteVoucher;
using Application.Vouchers.Commands.UpdateVoucher;
using Application.Vouchers.Queries.GetVoucherById;
using Application.Vouchers.Queries.GetVouchersPaginated;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

public class VouchersController: ApiControllerBase
{
    #region Basic CRUD Operations

    [HttpGet]
    public async Task<ActionResult<Result<PaginatedList<VoucherDto>>>> GetVouchers([FromQuery] PaginatedQueryParameters request)
    {
        var query = new GetVouchersPaginatedQuery()
        {
            Page = request.Page,
            Size = request.Size,
            SortBy = request.SortBy,
            SortOrder = request.SortOrder,
        };

        var result = await Mediator.Send(query);

        return Ok(Result<PaginatedList<VoucherDto>>.Succeed(result));
    }
    
    [HttpGet("{id:guid}")]
    public async Task<ActionResult<Result<VoucherDto>>> GetVoucherById([FromRoute] Guid id)
    {
        var query = new GetVoucherByIdQuery()
        {
            Id = id
        };

        var result = await Mediator.Send(query);

        return Ok(Result<VoucherDto>.Succeed(result));
    }
    
    [HttpPost]
    public async Task<ActionResult<Result<VoucherDto>>> AddVoucher([FromBody] AddVoucherRequest request)
    {
        var command = new CreateVoucherCommand()
        {
            VoucherCode = request.VoucherCode,
            DiscountValue = request.DiscountValue,
            Description = request.Description,
            ExpiredDate = request.ExpiredDate
        };

        var result = await Mediator.Send(command);

        return Ok(Result<VoucherDto>.Succeed(result));
    }
    
    [HttpPut("{id:guid}")]
    public async Task<ActionResult<Result<VoucherDto>>> UpdateVoucher([FromRoute] Guid id, [FromBody] UpdateVoucherRequest request)
    {
        var command = new UpdateVoucherCommand()
        {
            Id = id,
            VoucherCode = request.VoucherCode,
            DiscountValue = request.DiscountValue,
            Description = request.Description,
            ExpiredDate = request.ExpiredDate,
            IsActive = request.IsActive
        };

        var result = await Mediator.Send(command);

        return Ok(Result<VoucherDto>.Succeed(result));
    }
    
    [HttpDelete("{id:guid}")]
    public async Task<ActionResult<Result<VoucherDto>>> DeleteVoucher([FromRoute] Guid id)
    {
        var command = new DeleteVoucherCommand()
        {
            Id = id
        };

        var result = await Mediator.Send(command);

        return Ok(Result<VoucherDto>.Succeed(result));
    }

    #endregion
}