namespace Isitar.TimeTracking.Frontend.Services
{
    using System.Net.Http;
    using System.Threading.Tasks;

    public interface IGenericService
    {
        Task<HttpResponseMessage> GetAsyncRaw(string uri);
        Task<T> GetAsync<T>(string uri);
        Task<TResponse> PutAsync<TRequest, TResponse>(string uri, TRequest data);
        Task DeleteAsync(string uri);
        Task<TResponse> PostAsync<TRequest, TResponse>(string uri, TRequest data);
        Task<HttpResponseMessage> PostAsyncRaw<TRequest>(string uri, TRequest data);
        Task<HttpResponseMessage> PostAsyncRaw(string uri);
        Task<HttpResponseMessage> StopAsyncRaw<TRequest>(string uri, TRequest data);
        Task<HttpResponseMessage> StopAsyncRaw(string uri);
    }
}