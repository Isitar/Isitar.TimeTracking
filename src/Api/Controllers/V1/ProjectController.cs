namespace Isitar.TimeTracking.Api.Controllers.V1
{
    using System;
    using System.Threading.Tasks;
    using Application.Project.Commands.DeleteProject;
    using Application.Project.Queries.ProjectDetail;
    using Application.Project.Queries.ProjectImage;
    using Attributes;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.StaticFiles;
    using Routes.v1;

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

        [HttpGet(ApiRoutes.Project.Image, Name = nameof(ProjectController) + "/" + nameof(ImageAsync))]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesBadRequestResponse]
        public async Task<IActionResult> ImageAsync(Guid id)
        {
            var resp = await mediator.Send(new ProjectImageQuery
            {
                Id = id
            });
            var provider = new FileExtensionContentTypeProvider();
            if (!provider.TryGetContentType(resp.Filename, out var contentType))
            {
                contentType = "application/octet-stream";
            }

            return File(resp.Data, contentType, resp.Filename);
        }
        
        [HttpDelete(ApiRoutes.Project.Delete, Name = nameof(ProjectController) + "/" + nameof(DeleteAsync))]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesBadRequestResponse]
        public async Task<IActionResult> DeleteAsync(Guid id)
        {
             await mediator.Send(new DeleteProjectCommand {Id = id});
             return Ok();
        }
    }
}