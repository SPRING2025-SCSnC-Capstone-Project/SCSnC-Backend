using Application.Common.Models;
using Application.Common.Models.Dtos;
using Application.Payments.Queries.CheckPaymentResponse;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

public class PaymentController: ApiControllerBase
{
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