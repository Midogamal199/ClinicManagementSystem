using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ClinicManagementSystem.Application.Common;
using ClinicManagementSystem.Domain.Enums;

namespace ClinicManagementSystem.Domain.Entities
{
   public class Payment:BaseEntity
    {

        public decimal Amount { get; set; }
        public PaymentMethod Method { get; set; }
        public Guid InvoiceId { get; set; }
        public Invoice Invoice { get; set; }
    }
}
