using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ClinicManagementSystem.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ClinicManagementSystem.Infrastructure.Persistence.Conigurations
{
    public class PatientConfiguration : IEntityTypeConfiguration<Patient>
    {
        public void Configure(EntityTypeBuilder<Patient> builder)
        {
            builder.Property(p => p.FullName)
                 .IsRequired()
                 .HasMaxLength(150);
            builder.Property(p => p.PhoneNumber)
              .IsRequired()
              .HasMaxLength(20);
            builder.Property(p => p.Address)
            .HasMaxLength(250);

            builder.HasOne(p => p.MedicalFile)
               .WithOne(m => m.Patient)
               .HasForeignKey<MedicalFile>(m => m.PatientId)
               .OnDelete(DeleteBehavior.Cascade);

            builder.HasQueryFilter(p => !p.IsDeleted);


        }
    }
}
