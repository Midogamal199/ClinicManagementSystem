using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ClinicManagementSystem.Application.Common.Models;
using ClinicManagementSystem.Application.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace ClinicManagementSystem.Infrastructure.Identity
{
    public class IdentityService : IIdentityService
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public IdentityService(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }

        public async Task<AppIdentityResult> CreateStaffAccountAsync(string email, string password, string role, Guid employeeId)
        {
            var user = new ApplicationUser
            {
                UserName = email,
                Email = email,
                EmployeeId = employeeId
            };
            var result = await _userManager.CreateAsync(user, password);
            if (!result.Succeeded)
            {
                return new AppIdentityResult
                {
                    Succeeded = false,
                    Errors = result.Errors.Select(e => e.Description).ToList()
                };
            }
            await _userManager.AddToRoleAsync(user, role);

            return new AppIdentityResult
            {
                Succeeded = true,
                UserId = user.Id.ToString()
            };
        }

        public async Task<bool> EmployeeHasAccountAsync(Guid employeeId)
        {
            return await _userManager.Users.AnyAsync(u => u.EmployeeId == employeeId);
        }

        public async Task<bool> PatientHasAccountAsync(Guid patientId)
        {
            return await _userManager.Users.AnyAsync(u => u.PatientId == patientId);
        }

        public async Task<AppIdentityResult> RegisterPatientAsync(string email, string password, Guid patientId)
        {
            var user = new ApplicationUser
            {
                UserName = email,
                Email = email,
                PatientId = patientId
            };
            var result = await _userManager.CreateAsync(user, password);
            if (!result.Succeeded)
            {
                return new AppIdentityResult
                {
                    Succeeded = false,
                    Errors = result.Errors.Select(e => e.Description).ToList()
                };
            }
            await _userManager.AddToRoleAsync(user, Roles.Patient);

            return new AppIdentityResult
            {
                Succeeded = true,
                UserId = user.Id.ToString()
            };
        }

        public async Task<bool> UserExistsAsync(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);
            return user is not null;
        }
    }
}
