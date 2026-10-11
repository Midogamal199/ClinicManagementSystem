using System;
using System.Collections.Generic;

namespace ClinicManagementSystem.Application.DTOs.Prescriptions
{
    public class PrescriptionPdfModel
    {
        public Guid PrescriptionId { get; set; }
        public DateTime IssuedAt { get; set; }
        public string PatientFullName { get; set; } = string.Empty;
        public string DoctorFullName { get; set; } = string.Empty;
        public string DoctorLicenseNumber { get; set; } = string.Empty;
        public List<PrescriptionItemDto> Items { get; set; } = new();
    }

    public class PrescriptionPdfResult
    {
        public byte[] Content { get; set; } = Array.Empty<byte>();
        public string FileName { get; set; } = string.Empty;
    }
}