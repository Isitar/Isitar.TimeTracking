namespace Isitar.TimeTracking.Frontend.Services
{
    using System;
    using System.Threading.Tasks;
    using Application.Project.Queries.ProjectList;

    public class ProjectService : IProjectService
    {
        private readonly IGenericService genericService;

        public ProjectService(IGenericService genericService)
        {
            this.genericService = genericService;
        }

        public async Task<string> ProjectImageAsync(Guid id)
        {
            var resp = await genericService.GetAsyncRaw($"project/{id.ToString()}/image");
            var bytes = await resp.Content.ReadAsByteArrayAsync();
            var mimeType = resp.Content.Headers.ContentType.MediaType;
            var base64 = Convert.ToBase64String(bytes, Base64FormattingOptions.None);
            return $"data:{mimeType};base64,{base64}";
        }

        public Task<ProjectListVm> ProjectsForUserAsync(Guid userId)
        {
            return genericService.GetAsync<ProjectListVm>($"user/{userId.ToString()}/project");
            //return httpClient.GetAsJsonAsync<ProjectListVm>($"user/{userId.ToString()}/project", localStorageService, jsonSerializerOptions);
        }
    }
}