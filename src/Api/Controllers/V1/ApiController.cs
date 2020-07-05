namespace Isitar.TimeTracking.Api.Controllers.V1
{
    using MediatR;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.DependencyInjection;

    [ApiController]
    [Authorize]
    public class ApiController : ControllerBase
    {
        protected IMediator mediator => HttpContext.RequestServices.GetService<IMediator>();
    }
}