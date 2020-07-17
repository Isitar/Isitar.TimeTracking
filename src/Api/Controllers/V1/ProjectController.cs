namespace Isitar.TimeTracking.Api.Controllers.V1
{
    using System;
    using System.Threading.Tasks;
    using Application.Project.Commands.DeleteProject;
    using Application.Project.Commands.UpdateProjectName;
    using Application.Project.Queries.ProjectDetail;
    using Attributes;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;
    using Requests.V1.Project;
    using Routes.V1;

    public class ProjectController : ApiController
    {
        [HttpGet(ApiRoutes.Project.Single, Name = nameof(ProjectController) + "/" + nameof(SingleAsync))]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesBadRequestResponse]
        public async Task<IActionResult> SingleAsync(Guid id)
        {
            var resp = await mediator.Send(new ProjectDetailQuery
            {
                Id = id
            });
            return Ok(resp);
        }


        [HttpDelete(ApiRoutes.Project.Delete, Name = nameof(ProjectController) + "/" + nameof(DeleteAsync))]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesBadRequestResponse]
        public async Task<IActionResult> DeleteAsync(Guid id)
        {
            await mediator.Send(new DeleteProjectCommand {Id = id});
            return Ok();
        }

        [HttpPatch(ApiRoutes.Project.Update, Name = nameof(ProjectController) + "/" + nameof(UpdateAsync))]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesBadRequestResponse]
        public async Task<IActionResult> UpdateAsync(Guid id, UpdateProjectRequest updateProjectRequest)
        {
            await mediator.Send(new UpdateProjectNameCommand
            {
                Id = id,
                Name = updateProjectRequest.Name
            });
            return Ok();
        }
    }
}