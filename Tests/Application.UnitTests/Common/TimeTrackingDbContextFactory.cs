namespace Application.UnitTests.Common
{
    using System;
    using Isitar.TimeTracking.Infrastructure.Instant;
    using Isitar.TimeTracking.Persistence;
    using Microsoft.EntityFrameworkCore;

    public class TimeTrackingDbContextFactory
    {
        public static TimeTrackingDbContext Create()
        {
            var options = new DbContextOptionsBuilder<TimeTrackingDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            var context = new TimeTrackingDbContext(new CurrentUserServiceStub(), new SystemClockInstant(), options);
            return context;
        }

        public static void Destroy(TimeTrackingDbContext dbContext)
        {
            dbContext.Database.EnsureDeleted();
            dbContext.Dispose();
        }
    }
}