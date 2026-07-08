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
    public class VisitConfiguration : IEntityTypeConfiguration<Visit>
    {
        public void Configure(EntityTypeBuilder<Visit> builder)
        {
            builder.Property(v => v.VisitDate)
              .IsRequired();

            builder.Property(v => v.Notes)
                .HasMaxLength(1000);

            builder.HasMany(v => v.Diagnoses)
                .WithOne(d => d.Visit)
                .HasForeignKey(d => d.VisitId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(v => v.Prescriptions)
               .WithOne(p => p.Visit)
               .HasForeignKey(p => p.VisitId)
               .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(v => v.LabTests)
               .WithOne(l => l.Visit)
               .HasForeignKey(l => l.VisitId)
               .OnDelete(DeleteBehavior.Cascade);

            builder.HasQueryFilter(v => !v.IsDeleted);
        }
    }
}
