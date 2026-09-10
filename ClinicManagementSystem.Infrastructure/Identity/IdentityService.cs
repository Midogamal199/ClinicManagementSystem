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
        private readonly SignInManager<ApplicationUser> _signInManager;


        public IdentityService(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
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

        public async Task<AppIdentityResult> LoginAsync(string email, string password)
        {
            var user = await _userManager.Users
              .Include(u => u.Employee)
              .Include(u => u.Patient)
              .FirstOrDefaultAsync(u => u.Email == email);
            if (user is null)
            {
                return new AppIdentityResult
                {
                    Succeeded = false,
                    Errors = new List<string> { "Invalid email or password." }
                };

            }
            var signInResult = await _signInManager.CheckPasswordSignInAsync(user, password, lockoutOnFailure: false);
            if (!signInResult.Succeeded)
            {
                return new AppIdentityResult { Succeeded = false, Errors = new List<string> { "Invalid email or password." } };
            }
            var roles = await _userManager.GetRolesAsync(user);
            var fullName = user.Employee?.FullName ?? user.Patient?.FullName ?? user.Email!;
            return new AppIdentityResult
            {
                Succeeded = true,
                UserId = user.Id.ToString(),
                FullName = fullName,
                Roles = roles.ToList()
            };
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
