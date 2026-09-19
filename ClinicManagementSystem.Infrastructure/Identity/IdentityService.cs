using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ClinicManagementSystem.Application.Common.Models;
using ClinicManagementSystem.Application.Interfaces;
using ClinicManagementSystem.Infrastructure.Persistence;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace ClinicManagementSystem.Infrastructure.Identity
{
    public class IdentityService : IIdentityService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly ApplicationDbContext _context;


        public IdentityService(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, ApplicationDbContext context)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _context = context;

        }

        public async Task<AppIdentityResult> ChangePasswordAsync(Guid userId, string currentPassword, string newPassword)
        {
           var user =await _userManager.FindByIdAsync(userId.ToString());
            if (user is null)
            {
                return new AppIdentityResult { Succeeded = false, Errors = new List<string> { "User not found." } };
            }
            var resualt = await _userManager.ChangePasswordAsync(user, currentPassword, newPassword);
            if (!resualt.Succeeded)
            {
                return new AppIdentityResult
                {
                    Succeeded = false,
                    Errors = resualt.Errors.Select(e => e.Description).ToList()
                };
            }
            return new AppIdentityResult { Succeeded = true };

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

        public async Task<Guid?> GetUserIdByEmailAsync(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);
            return user?.Id;
        }

        public async Task<AppIdentityResult> GetUserInfoAsync(Guid userId)
        {
            var user = await _userManager.Users
                .Include(u => u.Employee)
                .Include(u => u.Patient)
                .FirstOrDefaultAsync(u => u.Id == userId);
            if (user is null)
            {
                return new AppIdentityResult { Succeeded = false, Errors = new List<string> { "User not found." } };
            }
            var roles = await _userManager.GetRolesAsync(user);
            var fullName = user.Employee?.FullName ?? user.Patient?.FullName ?? user.Email!;
            return new AppIdentityResult
            {
                Succeeded = true,
                UserId = user.Id.ToString(),
                Email = user.Email!,
                FullName = fullName,
                Roles = roles.ToList(),
                PatientId = user.PatientId,
                EmployeeId = user.EmployeeId
            };

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
                Roles = roles.ToList(),
                PatientId = user.PatientId,
                EmployeeId = user.EmployeeId
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

        public async Task<AppIdentityResult> ResetPasswordAsync(string email, string newPassword)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user is null)
            {
                return new AppIdentityResult { Succeeded = false, Errors = new List<string> { "User not found." } };
            }
            var removeResult = await _userManager.RemovePasswordAsync(user);
            if (!removeResult.Succeeded)
            {
                return new AppIdentityResult
                {
                    Succeeded = false,
                    Errors = removeResult.Errors.Select(e => e.Description).ToList()
                };
            }
            var addResult = await _userManager.AddPasswordAsync(user, newPassword);
            if (!addResult.Succeeded)
            {
                return new AppIdentityResult
                {
                    Succeeded = false,
                    Errors = addResult.Errors.Select(e => e.Description).ToList()
                };
            }
            await _userManager.UpdateSecurityStampAsync(user);
            return new AppIdentityResult { Succeeded = true };

        }

        public async Task RevokeAllUserTokensAsync(Guid userId)
        {
            var activeTokens = await _context.RefreshTokens
                .Where(rt => rt.UserId == userId &&!rt.IsRevoked ).ToListAsync();
            foreach (var token in activeTokens)
            {
                token.IsRevoked = true;
                token.RevokedAt = DateTime.UtcNow;
            }

            await _context.SaveChangesAsync();
        }

        public async Task StoreRefreshTokenAsync(Guid userId, string token, DateTime expiresAt)
        {
            var refreshToken = new RefreshToken
            {
                Id = Guid.NewGuid(),
                Token = token,
                UserId = userId,
                ExpiresAt = expiresAt
            };
            _context.RefreshTokens.Add(refreshToken);
            await _context.SaveChangesAsync();

        }

        public async Task<bool> UserExistsAsync(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);
            return user is not null;
        }

        public async Task<Guid?> ValidateAndConsumeRefreshTokenAsync(string token)
        {
            var storedToken = await _context.RefreshTokens.FirstOrDefaultAsync(t => t.Token == token);

            if (storedToken is null)
            {
                return null;
            }

            if (storedToken.IsRevoked)
            {
                await RevokeAllUserTokensAsync(storedToken.UserId);
                return null;
            }

            if (storedToken.ExpiresAt < DateTime.UtcNow)
            {
                return null;
            }

            storedToken.IsRevoked = true;
            storedToken.RevokedAt = DateTime.UtcNow;
            await _context.SaveChangesAsync();

            return storedToken.UserId;
        }
    }
}
