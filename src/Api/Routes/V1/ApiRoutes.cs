namespace Isitar.TimeTracking.Api.Routes.V1
{
    public class ApiRoutes
    {
        private const string Root = "api";

        private const string Version = "v1";

        private const string Base = Root + "/" + Version;

        public static class Auth
        {
            private const string AuthBase = Base + "/auth";
            public const string Login = AuthBase + "/login";
            public const string Logout = AuthBase + "/logout";
            public const string Refresh = AuthBase + "/refresh";

            public const string ChangePassword = AuthBase + "/{id}/password";
            public const string ChangeUsername = AuthBase + "/{id}/username";

            public const string AddRole = AuthBase + "/{id}/role";
            public const string RemoveRole = AuthBase + "/{id}/role/{roleName}";
            public const string UserRoles = AuthBase + "/{id}/role";
        }

        public static class User
        {
            private const string UserBase = Base + "/user";
            public const string All = UserBase;
            public const string Create = UserBase;
            public const string Update = UserBase + "/{id}";
            public const string Delete = UserBase + "/{id}";
            public const string Single = UserBase + "/{id}";

            public const string CreateProject = UserBase + "/{id}/project";
            public const string AllProjects = UserBase + "/{id}/project";

            public const string AllTimeTrackingEntries = UserBase + "/{id}/time-tracking-entry";
            public const string CurrentTimeTrackingEntry = UserBase + "/{id}/time-tracking-entry/current";
            public const string StartTracking = UserBase + "/{id}/time-tracking-entry";
        }

        public static class Project
        {
            private const string ProjectBase = Base + "/project";
            public const string All = ProjectBase;
            public const string Update = ProjectBase + "/{id}";
            public const string Delete = ProjectBase + "/{id}";
            public const string Single = ProjectBase + "/{id}";
            public const string Image = ProjectBase + "/{id}/image";
            public const string ReplaceImage = ProjectBase + "/{id}/image";
            public const string RemoveImage = ProjectBase + "/{id}/image";

            public const string AllTimeTrackingEntries = ProjectBase + "/{id}/time-tracking-entry";
        }

        public static class TimeTracking
        {
            private const string TimeTrackingBase = Base + "/time-tracking-entry";
            public const string All = TimeTrackingBase;
            public const string Create = TimeTrackingBase;
            public const string Update = TimeTrackingBase + "/{id}";
            public const string Delete = TimeTrackingBase + "/{id}";
            public const string Single = TimeTrackingBase + "/{id}";
        }
    }
}