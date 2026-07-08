using ClinicManagementSystem.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ClinicManagementSystem.Infrastructure.Persistence.Configurations
{
    public class DoctorConfiguration : IEntityTypeConfiguration<Doctor>
    {
        public void Configure(EntityTypeBuilder<Doctor> builder)
        {
            builder.Property(d => d.LicenseNumber)
                .IsRequired()
                .HasMaxLength(50);

           
            builder.HasOne(d => d.Employee)
                .WithOne(e => e.Doctor)
                .HasForeignKey<Doctor>(d => d.EmployeeId)
                .OnDelete(DeleteBehavior.Cascade);

        
            builder.HasMany(d => d.Specialties)
                .WithMany(s => s.Doctors)
                .UsingEntity(j => j.ToTable("DoctorSpecialties"));

           
            builder.HasMany(d => d.Appointments)
                .WithOne(a => a.Doctor)
                .HasForeignKey(a => a.DoctorId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasQueryFilter(d => !d.IsDeleted);
        }
    }
}