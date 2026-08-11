using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClinicManagementSystem.Application.DTOs.Invoices
{
    public class InvoiceDto
    {
        public Guid Id { get; set; }
        public decimal TotalAmount { get; set; }
        public decimal PaidAmount { get; set; }
        public decimal RemainingAmount { get; set; }
        public string Status { get; set; }
        public Guid PatientId { get; set; }
        public string PatientFullName { get; set; }
    }
}
