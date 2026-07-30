using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClinicManagementSystem.Application.DTOs.Diagnoses
{
    public class DiagnosisDto
    {
        public Guid Id { get; set; }
        public string Description { get; set; }
        public string IcdCode { get; set; }
        public Guid VisitId { get; set; }
        public string PatientFullName { get; set; }
    }
}
