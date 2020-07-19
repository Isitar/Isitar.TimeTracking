namespace Isitar.TimeTracking.Frontend.Services
{
    using System;
    using System.Threading.Tasks;
    using Application.Project.Queries.ProjectList;

    public interface IProjectService
    {
        public Task<string> ProjectImageAsync(Guid id);

        public Task<ProjectListVm> ProjectsForUserAsync(Guid userId);
    }
}