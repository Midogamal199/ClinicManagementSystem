using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace ClinicManagementSystem.Application.Interfaces
{
    public class FileUploadResult
    {
        public string StoredPath { get; set; }
        public string FileName { get; set; }
    }

    public interface IFileStorageService
    {
        Task<FileUploadResult> SaveFileAsync(IFormFile file, string subFolder);
        Task<Stream?> OpenReadAsync(string storedPath);

    }
}