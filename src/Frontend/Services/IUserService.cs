namespace Isitar.TimeTracking.Frontend.Services
{
    using System;
    using System.Threading.Tasks;
    using Application.Common.Entities;

    public interface IUserService
    {
        public Task<Result> StopTrackingAsync(Guid userId);
    }
}