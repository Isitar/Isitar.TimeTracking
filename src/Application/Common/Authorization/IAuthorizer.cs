namespace Isitar.TimeTracking.Application.Common.Authorization
{
    using System.Threading.Tasks;

    public interface IAuthorizer<T>
    {
        Task<bool> AuthorizeAsync(T request);
    }
}