namespace Isitar.TimeTracking.Api.Controllers.V1
{
    using System;
    using System.Threading.Tasks;
    using Application.User.Commands.CreateUser;
    using Application.User.Commands.CreateUserCommand;
    using Application.User.Queries.UserDetail;
    using Attributes;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;
    using Requests.V1.User;
    using Routes.v1;

    public class UserController : ApiController
    {
        [HttpGet(ApiRoutes.User.Single, Name = nameof(UserController) + "/" + nameof(SingleAsync))]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesBadRequestResponse]
        public async Task<IActionResult> SingleAsync(Guid id)
        {
            var resp = await mediator.Send(new UserDetailQuery()
            {
                Id = id
            });
            return Ok(resp);
        }

        [HttpPost(ApiRoutes.User.Create, Name = nameof(UserController) + "/" + nameof(CreateAsync))]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesBadRequestResponse]
        public async Task<IActionResult> CreateAsync(CreateUserRequest createUserRequest)
        {
            var id = Guid.NewGuid();
            await mediator.Send(new CreateUserCommand
            {
                Id = id,
                Name = createUserRequest.Name,
                Email = createUserRequest.Email,
                Acronym = createUserRequest.Acronym,
                Locale = "de-CH",
                Password = createUserRequest.Password,
            });
            return CreatedAtRoute($"{nameof(UserController)}/{nameof(SingleAsync)}",new {id},null);
        }
    }
}