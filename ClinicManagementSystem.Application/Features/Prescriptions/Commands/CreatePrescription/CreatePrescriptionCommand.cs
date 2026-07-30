using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;

namespace ClinicManagementSystem.Application.Features.Prescriptions.Commands.CreatePrescription
{
   public class CreatePrescriptionCommand:IRequest<Guid>
    {
        public Guid VisitId { get; set; }
        public List<PrescriptionItemInput> Items { get; set; } = new();

    }
    public class PrescriptionItemInput
    {
        public string MedicineName { get; set; }
        public string Dosage { get; set; }
    }
}
