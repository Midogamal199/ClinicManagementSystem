using System.Collections.Generic;

namespace ClinicManagementSystem.Application.Common.Models
{
    public class AppIdentityResult
    {
        public bool Succeeded { get; set; }
        public string UserId { get; set; } = string.Empty;
        public List<string> Errors { get; set; } = new();
    }
}