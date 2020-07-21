namespace Isitar.TimeTracking.Frontend.Services
{
    using System;
    using System.Net.Http;
    using System.Net.Http.Headers;
    using System.Text;
    using System.Text.Json;
    using System.Threading.Tasks;
    using Blazored.LocalStorage;
    using Common;
    using Exceptions;
    using Microsoft.AspNetCore.Http;
    using Microsoft.Extensions.Logging;

    public class GenericService : IGenericService
    {
        private readonly HttpClient httpClient;
        private readonly ILocalStorageService localStorageService;
        private readonly JsonSerializerOptions jsonSerializerOptions;
        private readonly IAuthService authService;
        private readonly ILogger<GenericService> logger;

        private static object lockObj = new object();


        public GenericService(HttpClient httpClient,
            ILocalStorageService localStorageService,
            JsonSerializerOptions jsonSerializerOptions,
            IAuthService authService,
            ILogger<GenericService> logger)
        {
            this.httpClient = httpClient;
            this.localStorageService = localStorageService;
            this.jsonSerializerOptions = jsonSerializerOptions;
            this.authService = authService;
            this.logger = logger;
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

        public Task<HttpResponseMessage> PostAsyncRaw<TRequest>(string uri, TRequest data)
        {
            return CallApiRaw(HttpMethod.Post, uri, data);
        }

        public Task<HttpResponseMessage> PostAsyncRaw(string uri)
        {
            return CallApiRaw(HttpMethod.Post, uri);
        }

        public Task<HttpResponseMessage> StopAsyncRaw<TRequest>(string uri, TRequest data)
        {
            return CallApiRaw(new HttpMethod("STOP"), uri, data);
        }

        public Task<HttpResponseMessage> StopAsyncRaw(string uri)
        {
            return CallApiRaw(new HttpMethod("STOP"), uri);
        }

        public async Task<TResponse> PutAsync<TRequest, TResponse>(string uri, TRequest data)
        {
            return await CallApi<TResponse>(HttpMethod.Put, uri, data);
        }

        private async Task<HttpRequestMessage> BuildRequestMessage(HttpMethod method, string uri, object data = null)
        {
            var request = new HttpRequestMessage(method, uri);
            request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            if (null != data)
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
                    }
                }

                return response;
            }
            catch (Exception e)
            {
                logger.LogError(e, "Exception while calling service");
                throw;
            }
        }

        private async Task<T> CallApi<T>(HttpMethod method, string uri, object data = null)
        {
            try
            {
                var response = await CallApiRaw(method, uri, data);
                var responseBody = await response.Content.ReadAsStringAsync();
                if (!response.IsSuccessStatusCode)
                {
                    if ((int) response.StatusCode == StatusCodes.Status404NotFound)
                    {
                        throw new HttpNotFoundException(responseBody);
                    }
                }

                var json = JsonSerializer.Deserialize<T>(responseBody, jsonSerializerOptions);
                return json;
            }
            catch (HttpNotFoundException)
            {
                throw;
            }
            catch (Exception e)
            {
                logger.LogError(e, "Exception while parsing answer to json");
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