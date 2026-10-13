using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ClinicManagementSystem.Application.DTOs.Prescriptions;

namespace ClinicManagementSystem.Application.DTOs.MedicalFiles
{
    public class MedicalFileDto
    {
        public Guid PatientId { get; set; }
        public string FullName { get; set; } = string.Empty;
        public DateTime DateOfBirth { get; set; }
        public string Gender { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
        public List<MedicalFileVisitDto> Visits { get; set; } = new();
    }

    public class MedicalFileVisitDto
    {
        public Guid VisitId { get; set; }
        public DateTime VisitDate { get; set; }
        public string Notes { get; set; } = string.Empty;
        public string DoctorFullName { get; set; } = string.Empty;
        public List<MedicalFileDiagnosisDto> Diagnoses { get; set; } = new();
        public List<MedicalFilePrescriptionDto> Prescriptions { get; set; } = new();
        public List<MedicalFileLabTestDto> LabTests { get; set; } = new();
    }

    public class MedicalFileLabTestDto
    {
        public Guid Id { get; set; }
        public string TestType { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;
        public string? ResultFileUrl { get; set; }
        public string? ResultFileName { get; set; }
        public DateTime? UploadedAt { get; set; }

    }


    public class MedicalFilePrescriptionDto
    {
        public Guid Id { get; set; }
        public DateTime IssuedAt { get; set; }
        public List<PrescriptionItemDto> Items { get; set; } = new();
    }

    public class MedicalFileDiagnosisDto
    {

        public Guid Id { get; set; }
        public string Description { get; set; } = string.Empty;
        public string IcdCode { get; set; } = string.Empty;
    }
}
