namespace Isitar.TimeTracking.Application.Project.Queries.ProjectImage
{
    using System.IO;

    public class ProjectImageDto 
    {
        public Stream Data { get; set; }
        public string Filename { get; set; }
    }
}