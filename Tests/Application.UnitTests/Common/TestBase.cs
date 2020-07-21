namespace Application.UnitTests.Common
{
    using System;
    using AutoMapper;
    using Isitar.TimeTracking.Application.Common.Mappings;
    using Isitar.TimeTracking.Persistence;

    public class TestBase : IDisposable
    {
        protected readonly TimeTrackingDbContext TimeTrackingDbContext;
        protected readonly IMapper Mapper;

        public TestBase()
        {
            TimeTrackingDbContext = TimeTrackingDbContextFactory.Create();
            var configurationProvider = new MapperConfiguration(cfg => { cfg.AddProfile<MappingProfile>(); });

            Mapper = configurationProvider.CreateMapper();
        }


        public void Dispose()
        {
            TimeTrackingDbContextFactory.Destroy(TimeTrackingDbContext);
        }
    }
}