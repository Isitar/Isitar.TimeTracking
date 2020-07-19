namespace Isitar.TimeTracking.Api.Controllers.V1
{
    using System;
    using System.Threading.Tasks;
    using Application.Common.Interfaces;
    using Application.TimeTrackingEntry.Queries.TimeTrackingEntryDetail;
    using Attributes;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;
    using Routes.V1;

    public class TimeTrackingController : ApiController
    {
        private readonly ICurrentUserService currentUserService;

        public TimeTrackingController(ICurrentUserService currentUserService)
        {
            this.currentUserService = currentUserService;
        }

        [HttpGet(ApiRoutes.TimeTracking.Single, Name = nameof(TimeTrackingController) + "/" + nameof(SingleAsync))]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesBadRequestResponse]
        public async Task<IActionResult> SingleAsync(Guid id)
        {
            var resp = await mediator.Send(new TimeTrackingEntryDetailQuery
            {
                Id = id
            });
            return Ok(resp);
        }
    }
}