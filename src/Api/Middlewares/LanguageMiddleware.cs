namespace Isitar.TimeTracking.Api.Middlewares
{
    using System.Globalization;
    using System.Threading.Tasks;
    using Application.Common.Interfaces;
    using Application.User.Queries.UserLocale;
    using MediatR;
    using Microsoft.AspNetCore.Http;

    public class LanguageMiddleware
    {
        private readonly RequestDelegate next;

        public LanguageMiddleware(RequestDelegate next)
        {
            this.next = next;
        }

        public async Task InvokeAsync(HttpContext httpContext, ICurrentUserService currentUserService, IMediator mediator)
        {
            CultureInfo.CurrentCulture = new CultureInfo("de-ch");
            if (currentUserService.IsAuthenticated)
            {
                var coreUserId = currentUserService.UserId;
                if (coreUserId.HasValue)
                {
                    var locale = await mediator.Send(new UserLocaleQuery {Id = coreUserId.Value});
                    var cultureInfo = new CultureInfo(locale.Locale);
                    CultureInfo.CurrentCulture = cultureInfo;
                    CultureInfo.CurrentUICulture = cultureInfo;
                }
            }

            await next(httpContext);
        }
    }
}