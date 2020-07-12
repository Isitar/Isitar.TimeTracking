namespace Isitar.TimeTracking.Api.Controllers.V1
{
    using System;
    using System.Diagnostics;
    using System.Threading.Tasks;
    using Application.Common.Interfaces;
    using Application.TimeTrackingEntry.Commands.StartTrackingForProject;
    using Application.TimeTrackingEntry.Queries.TimeTrackingEntryDetail;
    using Attributes;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;
    using Requests.V1.TimeTracking;
    using Routes.v1;

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

        [HttpPost(ApiRoutes.TimeTracking.Create, Name = nameof(TimeTrackingController) + "/" + nameof(CreateAsync))]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesBadRequestResponse]
        public async Task<IActionResult> CreateAsync(CreateTimeTrackingRequest createTimeTrackingRequest)
        {
            var trackingId = Guid.NewGuid();
            Debug.Assert(currentUserService.UserId != null, "currentUserService.UserId != null");
            await mediator.Send(new StartTrackingForProjectCommand
            {
                Id = trackingId,
                ProjectId = createTimeTrackingRequest.ProjectId,
                UserId = currentUserService.UserId.Value,
            });
            return CreatedAtRoute($"{nameof(ProjectController)}/{nameof(ProjectController.SingleAsync)}", new {id = trackingId}, null);
        }
    }
}