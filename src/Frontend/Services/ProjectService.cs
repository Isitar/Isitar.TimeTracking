namespace Isitar.TimeTracking.Frontend.Services
{
    using System;
    using System.Net.Http;
    using System.Threading.Tasks;
    using Blazored.LocalStorage;
    using Common;

    public class ProjectService : IProjectService
    {
        private readonly HttpClient httpClient;
        private readonly ILocalStorageService localStorageService;

        public ProjectService(HttpClient httpClient, ILocalStorageService localStorageService)
        {
            this.httpClient = httpClient;
            this.localStorageService = localStorageService;
        }

        public async Task<string> ProjectImageAsync(Guid id)
        {
            var resp = await httpClient.GetAuthorizedAsync($"project/{id.ToString()}/image", localStorageService);
            var bytes = await resp.Content.ReadAsByteArrayAsync();
            var mimeType = resp.Content.Headers.ContentType.MediaType;
            var base64 = Convert.ToBase64String(bytes, Base64FormattingOptions.None);
            return $"data:{mimeType};base64,{base64}";
        }
    }
}