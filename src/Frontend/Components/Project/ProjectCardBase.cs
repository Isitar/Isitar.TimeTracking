namespace Isitar.TimeTracking.Frontend.Components.Project
{
    using System.Threading.Tasks;
    using Application.Project.Queries.ProjectList;
    using Microsoft.AspNetCore.Components;
    using Services;

    public class ProjectCardBase : ComponentBase
    {
        [Parameter] public ProjectSlimDto Project { get; set; }

        [Inject] protected IProjectService ProjectService { get; set; }

        public string ProjectImageUrl { get; set; } = "images/projectplaceholder.png";

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender && Project.HasImage)
            {
                ProjectImageUrl = await ProjectService.ProjectImageAsync(Project.Id);
                StateHasChanged();
            }
        }
    }
}