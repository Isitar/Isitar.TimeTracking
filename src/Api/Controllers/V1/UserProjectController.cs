namespace Isitar.TimeTracking.Api.Controllers.V1
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Threading.Tasks;
    using Application.Project.Commands.CreateProject;
    using Application.Project.Queries.ProjectList;
    using Attributes;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;
    using Requests.V1.UserProject;
    using Routes.v1;

    public class UserProjectController : ApiController
    {
        [HttpPost(ApiRoutes.User.CreateProject, Name = nameof(UserProjectController) + "/" + nameof(CreateProjectAsync))]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesBadRequestResponse]
        public async Task<IActionResult> CreateProjectAsync(Guid id,  [FromForm] CreateProjectRequest createUserRequest)
        {
            var projectId = Guid.NewGuid();
            var hasImage = null != createUserRequest.Image;
            var ms = hasImage ?  new MemoryStream() : null;
            if (hasImage)
            {
                await createUserRequest.Image.CopyToAsync(ms);
            }

            await mediator.Send(new CreateProjectForUserCommand
            {
                Id = projectId,
                Name = createUserRequest.Name,
                ImageStream = ms,
                ImageStreamFilename = hasImage ? createUserRequest.Image.FileName : null,
                UserId = id
            });
            return CreatedAtRoute($"{nameof(ProjectController)}/{nameof(ProjectController.SingleAsync)}", new {id = projectId}, null);
        }
        
        [HttpGet(ApiRoutes.User.AllProjects, Name = nameof(UserProjectController) + "/" + nameof(AllProjectsAsync))]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesBadRequestResponse]
        public async Task<IActionResult> AllProjectsAsync(Guid id)
        {
            var res = await mediator.Send(new ProjectListQuery
            {
                UserFilter = new HashSet<Guid> { id},
            });
            return Ok(res);
        }
    }
}