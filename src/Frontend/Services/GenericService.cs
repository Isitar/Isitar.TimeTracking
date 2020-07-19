namespace Isitar.TimeTracking.Frontend.Services
{
    using System;
    using System.Diagnostics;
    using System.Net.Http;
    using System.Net.Http.Headers;
    using System.Text;
    using System.Text.Json;
    using System.Threading.Tasks;
    using Blazored.LocalStorage;
    using Common;

    public class GenericService : IGenericService
    {
        private readonly HttpClient httpClient;
        private readonly ILocalStorageService localStorageService;
        private readonly JsonSerializerOptions jsonSerializerOptions;
        private readonly IAuthService authService;

        private static object lockObj = new object();


        public GenericService(HttpClient httpClient,
            ILocalStorageService localStorageService,
            JsonSerializerOptions jsonSerializerOptions,
            IAuthService authService)
        {
            this.httpClient = httpClient;
            this.localStorageService = localStorageService;
            this.jsonSerializerOptions = jsonSerializerOptions;
            this.authService = authService;
        }

        public Task<HttpResponseMessage> GetAsyncRaw(string uri)
        {
            return CallApiRaw(HttpMethod.Get, uri);
        }

        public async Task<T> GetAsync<T>(string uri)
        {
            return await CallApi<T>(HttpMethod.Get, uri);
        }

        public async Task<TResponse> PostAsync<TRequest, TResponse>(string uri, TRequest data)
        {
            return await CallApi<TResponse>(HttpMethod.Post, uri, data);
        }

        public async Task<TResponse> PutAsync<TRequest, TResponse>(string uri, TRequest data)
        {
            return await CallApi<TResponse>(HttpMethod.Put, uri, data);
        }

        private async Task<HttpRequestMessage> BuildRequestMessage(HttpMethod method, string uri, object data = null)
        {
            var request = new HttpRequestMessage(method, uri);
            request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            if (data != null)
            {
                request.Content = new StringContent(JsonSerializer.Serialize(data), Encoding.UTF8, "application/json");
            }

            var token = await localStorageService.GetItemAsync<string>(LocalStorageConstants.JwtTokenKey);

            if (!string.IsNullOrEmpty(token))
            {
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
            }

            return request;
        }

        private async Task<HttpResponseMessage> CallApiRaw(HttpMethod method, string uri, object data = null)
        {
            try
            {
                var response = await httpClient.SendAsync(await BuildRequestMessage(method, uri, data));
                if (response.IsSuccessStatusCode)
                {
                    return response;
                }

                if (response.Headers.Contains("Token-Expired"))
                {
                    var refreshResult = await authService.RefreshAsync();
                    if (refreshResult.Successful)
                    {
                        response = await httpClient.SendAsync(await BuildRequestMessage(method, uri, data));
                        if (response.IsSuccessStatusCode)
                        {
                            return response;
                        }
                    }
                }

                var err = await response.Content.ReadAsStringAsync();
                throw new Exception($"[{response.StatusCode}] error: {err}");
            }
            catch (Exception e)
            {
                Debug.WriteLine($"{e.GetType().Name} : {e.Message}");
                throw;
            }
        }

        private async Task<T> CallApi<T>(HttpMethod method, string uri, object data = null)
        {
            try
            {
                var response = await CallApiRaw(method, uri, data);
                var jsonResult = await response.Content.ReadAsStringAsync();
                var json = JsonSerializer.Deserialize<T>(jsonResult, jsonSerializerOptions);
                return json;
            }
            catch (Exception e)
            {
                Debug.WriteLine($"{e.GetType().Name} : {e.Message}");
                throw;
            }
        }

        public async Task DeleteAsync(string uri)
        {
            await httpClient.DeleteAsync(GetUri(uri));
        }

        private string GetUri(string uri) => $"{uri}";
    }
}