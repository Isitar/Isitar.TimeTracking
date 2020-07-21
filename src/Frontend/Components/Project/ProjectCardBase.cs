namespace Isitar.TimeTracking.Frontend.Components.Project
{
    using System;
    using System.Threading.Tasks;
    using Application.Project.Queries.ProjectList;
    using Frontend.Common.Authentication;
    using Microsoft.AspNetCore.Components;
    using Microsoft.AspNetCore.Components.Authorization;
    using Services;

    public class ProjectCardBase : ComponentBase
    {
        [Parameter] public ProjectSlimDto Project { get; set; }
        [Parameter] public EventCallback<Guid> WorkingOnChanged { get; set; }

        [Inject] protected IProjectService ProjectService { get; set; }
        [Inject] public IUserService UserService { get; set; }
        [Inject] public AuthenticationStateProvider AuthenticationStateProvider { get; set; }

        public string ProjectImageUrl { get; set; } = "images/projectplaceholder.png";

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender && Project.HasImage)
            {
                ProjectImageUrl = await ProjectService.ProjectImageAsync(Project.Id);
                StateHasChanged();
            }
        }

        protected async Task StartWorking()
        {
            var userId = await AuthenticationStateProvider.UserIdAsync();
            var startResult = await UserService.StartTrackingAsync(userId, Project.Id);
            if (startResult.Successful)
            {
                await WorkingOnChanged.InvokeAsync(Project.Id);
            }
        }
    }
}