namespace Isitar.TimeTracking.Api.Requests.V1.UserProject
{
    using System;
    using System.IO;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;

    public class CreateProjectRequest
    {
        public string Name { get; set; }
        [FromForm(Name = nameof(Image))]
        public IFormFile Image { get; set; }
    }
}