using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace ClinicManagementSystem.Application.Interfaces
{
    public class FileUploadResult
    {
        public string FileUrl { get; set; }
        public string FileName { get; set; }
    }

    public interface IFileStorageService
    {
        Task<FileUploadResult> SaveFileAsync(IFormFile file, string subFolder);
    }
}