namespace Isitar.TimeTracking.Api.Controllers.V1
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Application.TimeTrackingEntry.Commands.StartTrackingForProject;
    using Application.TimeTrackingEntry.Commands.StopTrackingForUser;
    using Application.TimeTrackingEntry.Queries.ActiveTimeTrackingEntryDetail;
    using Application.TimeTrackingEntry.Queries.TimeTrackingEntryList;
    using Attributes;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;
    using Requests.V1.TimeTracking;
    using Requests.V1.User;
    using Routes.V1;

    public class UserTimeTrackingEntryController : ApiController
    {
        [HttpGet(ApiRoutes.User.AllTimeTrackingEntries, Name = nameof(UserTimeTrackingEntryController) + "/" + nameof(AllTimeTrackingEntriesAsync))]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesBadRequestResponse]
        public async Task<IActionResult> AllTimeTrackingEntriesAsync(Guid id, [FromQuery] AllTimeTrackingEntriesFilter filter)
        {
            var resp = await mediator.Send(new TimeTrackingEntryListQuery
            {
                UserFilter = new HashSet<Guid> {id},
                From = filter.From,
                To = filter.To,
                ProjectFilter = filter.ProjectIds
            });
            return Ok(resp);
        }

        [HttpGet(ApiRoutes.User.CurrentTimeTrackingEntry, Name = nameof(UserTimeTrackingEntryController) + "/" + nameof(CurrentTimeTrackingEntryAsync))]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesBadRequestResponse]
        public async Task<IActionResult> CurrentTimeTrackingEntryAsync(Guid id)
        {
            var resp = await mediator.Send(new ActiveTimeTrackingEntryDetailQuery
            {
                UserId = id
            });
            return Ok(resp);
        }


        /// <summary>
        ///     Starts tracking for a new project
        ///     if this method is called with an empty object (-> no projectId present) the current tracking will be stopped
        /// </summary>
        /// <param name="id">the user id</param>
        /// <param name="startTimeTrackingRequest">the request </param>
        /// <returns></returns>
        [HttpPost(ApiRoutes.User.StartTracking, Name = nameof(UserTimeTrackingEntryController) + "/" + nameof(StartTrackingAsync))]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesBadRequestResponse]
        public async Task<IActionResult> StartTrackingAsync(Guid id, StartTimeTrackingRequest startTimeTrackingRequest)
        {
            if (startTimeTrackingRequest.ProjectId.HasValue)
            {
                var trackingId = Guid.NewGuid();
                await mediator.Send(new StartTrackingForProjectCommand
                {
                    Id = trackingId,
                    ProjectId = startTimeTrackingRequest.ProjectId.Value,
                    UserId = id,
                });
                return CreatedAtRoute($"{nameof(TimeTrackingController)}/{nameof(TimeTrackingController.SingleAsync)}", new {id = trackingId}, null);
            }
            else
            {
                await mediator.Send(new StopTrackingForUserCommand
                {
                    UserId = id,
                });
                return Ok();
            }
        }
    }
}