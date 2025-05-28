using Api.Controllers.Payload.Requests;
using Application.Blogs.Queries.GetBlogsPaginated;
using Application.Common.Models.Dtos;
using Application.Common.Models;
using Microsoft.AspNetCore.Mvc;
using Api.Controllers.Payload.Requests.Dashboard;
using Application.Dashboard.Queries;

namespace Api.Controllers
{
    public class DashboardController : ApiControllerBase
    {
        [HttpPost]
        public async Task<ActionResult<DashboardDto>> GetDashboard([FromBody] DashboardRequest request)
        {
            var query = new GetInformationForDashboardQuery()
            {
                GivenMonth = request.GivenMonth
            };

            var result = await Mediator.Send(query);

            return Ok(Result<DashboardDto>.Succeed(result));
        }
    }
}
