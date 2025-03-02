using Api.Controllers.Payload.Requests;
using Api.Controllers.Payload.Requests.Transactions;
using Application.Common.Models;
using Application.Common.Models.Dtos;
using Application.Transactions.Queries.GetTransactionById;
using Application.Transactions.Queries.GetTransactionsByDayPaginated;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

public class TransactionsController: ApiControllerBase
{
    [HttpGet]
    public async Task<ActionResult<Result<PaginatedList<TransactionDto>>>> GetTransactionsByDate([FromQuery] PaginatedQueryParameters request, [FromQuery] TransactionDateRequest date)
    {
        var query = new GetTransactionsByDayPaginatedQuery()
        {
            Page = request.Page,
            Size = request.Size,
            SortBy = request.SortBy,
            SortOrder = request.SortOrder,
            // Important note: Input dates don't need to be specific, "yyyy-mm-dd" should be fine
            StartDate = date.From,
            EndDate = date.To
        };

        var result = await Mediator.Send(query);

        return Ok(Result<PaginatedList<TransactionDto>>.Succeed(result));
    }
    
    [HttpGet("{id:guid}")]
    public async Task<ActionResult<Result<DetailTransactionDto>>> GetTransactionById([FromRoute] Guid id)
    {
        var query = new GetTransactionByIdQuery()
        {
            Id = id
        };

        var result = await Mediator.Send(query);

        return Ok(Result<DetailTransactionDto>.Succeed(result));
    }
}