using System;

namespace ClinicManagementSystem.Application.DTOs.Reports
{
    public class ReportFileResult
    {
        public byte[] Content { get; set; } = Array.Empty<byte>();
        public string ContentType { get; set; } = string.Empty;
        public string FileName { get; set; } = string.Empty;
    }
}