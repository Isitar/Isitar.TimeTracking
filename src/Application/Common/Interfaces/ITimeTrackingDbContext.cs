namespace Isitar.TimeTracking.Application.Common.Interfaces
{
    using System.Threading;
    using System.Threading.Tasks;
    using Domain.Entities;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Infrastructure;

    public interface ITimeTrackingDbContext
    {
        public DbSet<Project> Projects { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<TimeTrackingEntry> TimeTrackingEntries { get; set; }

        public Task<int> SaveChangesAsync(CancellationToken cancellationToken);

        public DatabaseFacade Database { get; }
    }
}