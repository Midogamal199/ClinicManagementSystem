using System;
using System.Collections.Generic;

namespace ClinicManagementSystem.Application.DTOs.Auth
{
    public class LoginResponseDto
    {
        public string Token { get; set; }
        public DateTime ExpiresAt { get; set; }
        public string RefreshToken { get; set; }

        public Guid UserId { get; set; }
        public string Email { get; set; }
        public string FullName { get; set; }
        public List<string> Roles { get; set; } = new();
    }
}