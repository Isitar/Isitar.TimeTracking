namespace Isitar.TimeTracking.Api.Routes.v1
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
            public const string RemoveRole = AuthBase + "/{id}/role";
            public const string UserRoles = AuthBase + "/{id}/role";
        }
    }
}