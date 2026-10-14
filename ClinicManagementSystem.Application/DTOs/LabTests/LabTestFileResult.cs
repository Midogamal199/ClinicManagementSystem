using System.IO;

namespace ClinicManagementSystem.Application.DTOs.LabTests
{
    public class LabTestFileResult
    {
        public Stream Content { get; set; } = Stream.Null;
        public string ContentType { get; set; } = "application/octet-stream";
        public string FileName { get; set; } = string.Empty;
    }
}