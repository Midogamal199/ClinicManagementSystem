using ClinicManagementSystem.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ClinicManagementSystem.Infrastructure.Persistence.Conigurations
{
    public class MedicalFileConfiguration : IEntityTypeConfiguration<MedicalFile>
    {
        public void Configure(EntityTypeBuilder<MedicalFile> builder)
        {
            builder.Property(m => m.LastUpdated)
                .IsRequired();

            builder.HasQueryFilter(m => !m.IsDeleted);
        }
    }
}