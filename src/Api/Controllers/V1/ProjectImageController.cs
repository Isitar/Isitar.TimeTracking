namespace Isitar.TimeTracking.Api.Controllers.V1
{
    using System;
    using System.IO;
    using System.Threading.Tasks;
    using Application.Project.Commands.ReplaceImage;
    using Application.Project.Queries.ProjectImage;
    using Attributes;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.StaticFiles;
    using Requests.V1.Project;
    using Routes.V1;

    public class ProjectImageController : ApiController
    {
        [HttpGet(ApiRoutes.Project.Image, Name = nameof(ProjectImageController) + "/" + nameof(ImageAsync))]
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

        [HttpPut(ApiRoutes.Project.ReplaceImage, Name = nameof(ProjectImageController) + "/" + nameof(ReplaceImageAsync))]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesBadRequestResponse]
        public async Task<IActionResult> ReplaceImageAsync(Guid id, [FromForm] ReplaceImageRequest replaceImageRequest)
        {
            var ms = new MemoryStream();
            await replaceImageRequest.Image.CopyToAsync(ms);

            await mediator.Send(new ReplaceImageCommand
            {
                Id = id,
                ImageStream = ms,
                ImageStreamFilename = replaceImageRequest.Image.FileName,
            });
            return Ok();
        }
    }
}