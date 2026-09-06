using System;

namespace ClinicManagementSystem.Application.DTOs.Specialties
{
    public class DoctorSummaryDto
    {
        public Guid Id { get; set; }
        public string FullName { get; set; }
        public string LicenseNumber { get; set; }
    }
}