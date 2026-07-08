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
    public class EmployeeConfiguration : IEntityTypeConfiguration<Employee>
    {
        public void Configure(EntityTypeBuilder<Employee> builder)
        {
            builder.Property(e => e.FullName)
             .IsRequired()
             .HasMaxLength(150);

            builder.Property(e => e.PhoneNumber)
                .IsRequired()
                .HasMaxLength(20);

            builder.Property(e => e.Salary)
                .HasColumnType("decimal(10,2)")
                .IsRequired();
            builder.HasMany(e=>e.Attendances)
                .WithOne(a=>a.Employee)
                .HasForeignKey(a=>a.EmployeeId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(e => e.LeaveRequests)
             .WithOne(l => l.Employee)
             .HasForeignKey(l => l.EmployeeId)
             .OnDelete(DeleteBehavior.Cascade);



        }
    }
}
