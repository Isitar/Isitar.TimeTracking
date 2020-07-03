namespace Isitar.TimeTracking.Persistence.Configurations
{
    using Domain.Entities;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;

    public class AuditTrailEntryConfiguration : IEntityTypeConfiguration<AuditTrailEntry>
    {
        public void Configure(EntityTypeBuilder<AuditTrailEntry> builder)
        {
            builder.HasOne(a => a.CreatedBy)
                .WithMany()
                .HasForeignKey(a => a.CreatedById)
                .OnDelete(DeleteBehavior.SetNull);
        }
    }
}