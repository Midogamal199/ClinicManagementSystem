using ClinicManagementSystem.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ClinicManagementSystem.Infrastructure.Persistence.Configurations
{
    public class PrescriptionItemConfiguration : IEntityTypeConfiguration<PrescriptionItem>
    {
        public void Configure(EntityTypeBuilder<PrescriptionItem> builder)
        {
            builder.Property(i => i.MedicineName)
                .IsRequired()
                .HasMaxLength(150);

            builder.Property(i => i.Dosage)
                .IsRequired()
                .HasMaxLength(100);

            builder.HasQueryFilter(i => !i.IsDeleted);
        }
    }
}