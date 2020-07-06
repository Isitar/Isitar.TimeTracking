namespace Application.UnitTests.User.Queries
{
    using System;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using Common;
    using Isitar.TimeTracking.Application.User.Queries.UserDetail;
    using Isitar.TimeTracking.Domain.Entities;
    using NodaTime;
    using Xunit;

    public class UserDetailQueryHandlerTests : TestBase
    {
        [Fact]
        public async Task CheckMapping()
        {
            var createdId = Guid.NewGuid();
            var updatedId = Guid.NewGuid();
            var id = Guid.NewGuid();
            await TimeTrackingDbContext.Users.AddAsync( new User
            {
                Acronym = "lpa",
                Name = "Pascal Lüscher",
                Email = "luescherpascal@gmail.com",
                Locale = "de-CH",
                Id = id,
                CreatedBy = new User
                {
                    Id = createdId,
                    Name = "creator"
                },
                CreatedById = createdId,
                CreatedAt = Instant.FromUtc(2020, 1, 1, 0, 0, 0),
                UpdatedBy = new User
                {
                    Id = updatedId,
                    Name = "updator"
                },
                UpdatedById = updatedId,
                UpdatedAt = Instant.FromUtc(2020, 1, 2, 0, 0, 0),
                AuditTrailEntries =
                {
                    new AuditTrailEntry
                    {
                        Id = Guid.NewGuid(),
                        When = Instant.FromUtc(2020, 1, 1, 0, 0, 0),
                        CreatedById = createdId,
                        CreatedBy = new User
                        {
                            Id = createdId,
                            Name = "creator"
                        },
                        NewValue = "hello",
                        OldValue = "world",
                    }
                }
            });
            await TimeTrackingDbContext.SaveChangesAsync();

            var q = new UserDetailQuery
            {
                Id = id
            };
            var queryHandler = new UserDetailQueryHandler(TimeTrackingDbContext, Mapper);
            var result = await queryHandler.Handle(q, CancellationToken.None);
            
            Assert.NotNull(result);
            Assert.Equal(2, result.AuditTrailEntries.Count());
        }
    }
}