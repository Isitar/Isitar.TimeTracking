namespace Isitar.TimeTracking.Api.Middlewares
{
    using System;
    using System.Globalization;
    using System.Threading.Tasks;
    using Application.Common.Interfaces;
    using Application.User.Queries.UserLocale;
    using MediatR;
    using Microsoft.AspNetCore.Http;

    public class LanguageMiddleware
    {
        public async Task Invoke(HttpContext httpContext, ICurrentUserService currentUserService, IMediator mediator, Func<Task> next)
        {
            CultureInfo.CurrentCulture = new CultureInfo("de-ch");
            if (!currentUserService.IsAuthenticated)
            {
                return;
            }

            var coreUserId = currentUserService.UserId;
            if (!coreUserId.HasValue)
            {
                return;
            }

            var locale = await mediator.Send(new UserLocaleQuery {Id = coreUserId.Value});
            var cultureInfo = new CultureInfo(locale.Locale);
            CultureInfo.CurrentCulture = cultureInfo;
            CultureInfo.CurrentUICulture = cultureInfo;
            
            await next.Invoke();
        }
    }
}