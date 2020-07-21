namespace Isitar.TimeTracking.Frontend.Services
{
    using System;
    using System.Threading.Tasks;
    using Application.Common.Entities;
    using Common;

    public class UserService : IUserService
    {
        private readonly IGenericService genericService;

        public UserService(IGenericService genericService)
        {
            this.genericService = genericService;
        }

        public async Task<Result> StopTrackingAsync(Guid userId)
        {
            var res = await genericService.PostAsyncRaw($"user/{userId}/time-tracking-entry", new object());
            if (res.IsSuccessStatusCode)
            {
                return Result.Success();
            }

            return Result.Failure(await res.ErrorMessagesAsync());
        }

        public async Task<Result> StartTrackingAsync(Guid userId, Guid projectId)
        {
            var res = await genericService.PostAsyncRaw($"user/{userId}/time-tracking-entry", new
            {
                ProjectId = projectId
            });
            if (res.IsSuccessStatusCode)
            {
                return Result.Success();
            }

            return Result.Failure(await res.ErrorMessagesAsync());
        }
    }
}