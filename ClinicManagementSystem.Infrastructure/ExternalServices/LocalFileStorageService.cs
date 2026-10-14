using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ClinicManagementSystem.Application.Interfaces;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;

namespace ClinicManagementSystem.Infrastructure.ExternalServices
{
    public class LocalFileStorageService : IFileStorageService
    {
        private const string PrivateRootFolderName = "PrivateUploads";
        private readonly string _rootPath;

        public LocalFileStorageService(IWebHostEnvironment webHostEnvironment)
        {
            _rootPath = Path.GetFullPath(
                            Path.Combine(webHostEnvironment.ContentRootPath, PrivateRootFolderName));
        }

        public Task<Stream?> OpenReadAsync(string storedPath)
        {
            var fullPath = Path.GetFullPath(Path.Combine(_rootPath, storedPath));

            var insideRoot = fullPath.StartsWith(
                _rootPath + Path.DirectorySeparatorChar,
                StringComparison.OrdinalIgnoreCase);

            if (!insideRoot || !File.Exists(fullPath))
            {
                return Task.FromResult<Stream?>(null);
            }

            Stream stream = new FileStream(fullPath, FileMode.Open, FileAccess.Read, FileShare.Read);
            return Task.FromResult<Stream?>(stream);
        }

        public async Task<FileUploadResult> SaveFileAsync(IFormFile file, string subFolder)
        {
            var folder = Path.Combine(_rootPath, subFolder);
            Directory.CreateDirectory(folder);
            var uniqueFileName = $"{Guid.NewGuid()}{Path.GetExtension(file.FileName)}";
            var fullPath = Path.Combine(folder, uniqueFileName);
            await using (var stream = new FileStream(fullPath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            return new FileUploadResult
            {
                StoredPath = $"{subFolder}/{uniqueFileName}",
                FileName = file.FileName
            };

        }
    }
}
