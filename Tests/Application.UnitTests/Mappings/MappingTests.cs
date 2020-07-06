namespace Application.UnitTests.Mappings
{
    using System;
    using AutoMapper;
    using Isitar.TimeTracking.Application.User.Queries.UserDetail;
    using Isitar.TimeTracking.Domain.Entities;
    using NodaTime;
    using Xunit;

    public class MappingTests : IClassFixture<MappingTestsFixture>
    {
        private readonly IConfigurationProvider configuration;
        private readonly IMapper mapper;

        public MappingTests(MappingTestsFixture fixture)
        {
            configuration = fixture.ConfigurationProvider;
            mapper = fixture.Mapper;
        }

        [Fact]
        public void ShouldHaveValidConfiguration()
        {
            configuration.AssertConfigurationIsValid();
        }

        [Fact]
        public void ShouldMapUserToUserDetailVm()
        {
            var createdId = Guid.NewGuid();
            var updatedId = Guid.NewGuid();
            var id = Guid.NewGuid();
            var entity = new User
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
            };
            var result = mapper.Map<UserDetailVm>(entity);
            Assert.NotNull(result);
            Assert.Equal("lpa", result.Acronym);
            Assert.Equal("Pascal Lüscher", result.Name);
            Assert.Equal("de-CH", result.Locale);
            Assert.Equal("creator", result.CreatedByName);
            Assert.Equal("updator", result.UpdatedByName);
            Assert.Equal(id, result.Id);
            Assert.Equal(Instant.FromUtc(2020, 1, 1, 0, 0, 0), result.CreatedAt);
            Assert.Equal(Instant.FromUtc(2020, 1, 2, 0, 0, 0), result.UpdatedAt);
            Assert.NotEmpty(result.AuditTrailEntries);
            Assert.Contains(result.AuditTrailEntries, entry => entry.When.Equals(Instant.FromUtc(2020, 1, 1, 0, 0, 0)));
        }
    }
}