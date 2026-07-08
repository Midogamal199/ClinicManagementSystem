using ClinicManagementSystem.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ClinicManagementSystem.Infrastructure.Persistence.Configurations
{
    public class LabTestConfiguration : IEntityTypeConfiguration<LabTest>
    {
        public void Configure(EntityTypeBuilder<LabTest> builder)
        {
            builder.Property(l => l.TestType)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(l => l.Status)
                .IsRequired()
                .HasConversion<string>()
                .HasMaxLength(20);

            builder.Property(l => l.ResultFileUrl)
                .HasMaxLength(300);

            builder.HasQueryFilter(l => !l.IsDeleted);
        }
    }
}