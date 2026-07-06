using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ClinicManagementSystem.Application.Common;
using ClinicManagementSystem.Domain.Enums;

namespace ClinicManagementSystem.Domain.Entities
{
    public class Invoice:BaseEntity
    {
        public decimal TotalAmount { get; set; }
        public InvoiceStatus Status { get; set; }
        public Guid PatientId { get; set; }
        public Patient Patient { get; set; }

        public ICollection<Payment> Payments { get; set; } = new List<Payment>();
    }
}
