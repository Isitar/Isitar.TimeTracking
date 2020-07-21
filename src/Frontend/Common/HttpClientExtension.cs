namespace Isitar.TimeTracking.Frontend.Common
{
    using System.Net.Http;
    using System.Net.Http.Headers;
    using System.Text.Json;
    using System.Threading.Tasks;
    using Blazored.LocalStorage;

    public static class HttpClientExtension
    {
        public static async Task<T> GetAsJsonAsync<T>(this HttpClient httpClient, string uri, ILocalStorageService localStorageService, JsonSerializerOptions jsonSerializerOptions)
        {
            var response = await GetAuthorizedAsync(httpClient, uri, localStorageService);
            return JsonSerializer.Deserialize<T>(await response.Content.ReadAsByteArrayAsync(), jsonSerializerOptions);
        }

        public static async Task<HttpResponseMessage> GetAuthorizedAsync(this HttpClient httpClient, string uri, ILocalStorageService localStorageService)
        {
            var request = new HttpRequestMessage(HttpMethod.Get, uri);
            var token = await localStorageService.GetItemAsync<string>(LocalStorageConstants.JwtTokenKey);
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
            return await httpClient.SendAsync(request);
        }
    }
}