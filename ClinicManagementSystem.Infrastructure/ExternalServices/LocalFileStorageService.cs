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
        private readonly IWebHostEnvironment _webHostEnvironment;

        public LocalFileStorageService(IWebHostEnvironment webHostEnvironment)
        {
            _webHostEnvironment = webHostEnvironment;
        }

        public async Task<FileUploadResult> SaveFileAsync(IFormFile file, string subFolder)
        {
            var uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, subFolder);
            Directory.CreateDirectory(uploadsFolder);
            var uniqueFileName = $"{Guid.NewGuid()}{Path.GetExtension(file.FileName)}";
            var filePath = Path.Combine(uploadsFolder, uniqueFileName);
            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }
            return new FileUploadResult
            {
                FileUrl = $"/uploads/{subFolder}/{uniqueFileName}",
                FileName = file.FileName
            };
        }
    }
}
