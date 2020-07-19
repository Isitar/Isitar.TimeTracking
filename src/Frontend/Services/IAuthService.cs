namespace Isitar.TimeTracking.Frontend.Services
{
    using System.Threading.Tasks;
    using Application.Common.Entities;

    public interface IAuthService
    {
        public Task<Result> LoginAsync(string username, string password);
        public Task LogoutAsync();

        public Task<Result> RefreshAsync();
    }
}