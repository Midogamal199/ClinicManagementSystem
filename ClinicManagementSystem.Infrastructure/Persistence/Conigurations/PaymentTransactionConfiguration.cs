using ClinicManagementSystem.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ClinicManagementSystem.Infrastructure.Persistence.Configurations
{
    public class PaymentTransactionConfiguration : IEntityTypeConfiguration<PaymentTransaction>
    {
        public void Configure(EntityTypeBuilder<PaymentTransaction> builder)
        {
            builder.Property(t => t.Amount)
                .HasColumnType("decimal(10,2)")
                .IsRequired();

            builder.Property(t => t.GatewayReference)
                .HasMaxLength(100);

            builder.Property(t => t.Status)
                .IsRequired()
                .HasConversion<string>()
                .HasMaxLength(20);

            builder.HasOne(t => t.Invoice)
                .WithMany(i => i.PaymentTransactions)
                .HasForeignKey(t => t.InvoiceId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(t => t.Payment)
                .WithOne()
                .HasForeignKey<PaymentTransaction>(t => t.PaymentId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasQueryFilter(t => !t.IsDeleted);
        }
    }
}