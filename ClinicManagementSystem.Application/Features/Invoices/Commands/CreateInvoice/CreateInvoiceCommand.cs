using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;

namespace ClinicManagementSystem.Application.Features.Invoices.Commands.CreateInvoice
{
    public class CreateInvoiceCommand: IRequest<Guid>
    {
        public Guid PatientId { get; set; }
        public decimal TotalAmount { get; set; }
    }
}
