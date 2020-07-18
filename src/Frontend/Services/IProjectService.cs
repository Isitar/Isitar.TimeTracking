namespace Isitar.TimeTracking.Frontend.Services
{
    using System;
    using System.Threading.Tasks;

    public interface IProjectService
    {
        public Task<string> ProjectImageAsync(Guid id);
    }
}