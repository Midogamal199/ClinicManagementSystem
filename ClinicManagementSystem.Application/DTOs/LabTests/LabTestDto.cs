using System;

namespace ClinicManagementSystem.Application.DTOs.LabTests
{
    public class LabTestDto
    {
        public Guid Id { get; set; }
        public string TestType { get; set; }
        public string Status { get; set; }
        public string? ResultFileUrl { get; set; }
        public string? ResultFileName { get; set; }
        public DateTime? UploadedAt { get; set; }
        public Guid VisitId { get; set; }
    }
}