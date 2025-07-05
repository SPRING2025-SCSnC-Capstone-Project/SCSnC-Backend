using Application.Common.Interfaces;
using Application.Common.Models;
using Application.Common.Models.Dtos;
using Application.Payments.Queries.CheckPaymentResponse;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace Api.Controllers;

public class PaymentController: ApiControllerBase
{
    private readonly IApplicationDbContext _context;

    public PaymentController(IApplicationDbContext context)
    {
        _context = context;
    }

    [HttpGet("result")]
    public async Task<ActionResult> PaymentReturn([FromQuery] VNPayResponse response)
    {
        var command = new CheckPaymentResponseQuery()
        {
            vnpayResponse = response
        };

        var result = await Mediator.Send(command);

        return Ok(Result<PaymentResponse>.Succeed(result));
    }
}