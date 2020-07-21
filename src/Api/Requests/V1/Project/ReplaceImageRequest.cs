namespace Isitar.TimeTracking.Api.Requests.V1.Project
{
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;

    public class ReplaceImageRequest
    {
        [FromForm(Name = nameof(Image))] public IFormFile Image { get; set; }
    }
}