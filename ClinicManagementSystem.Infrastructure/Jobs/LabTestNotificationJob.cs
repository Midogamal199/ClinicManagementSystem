using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ClinicManagementSystem.Application.Interfaces;
using ClinicManagementSystem.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace ClinicManagementSystem.Infrastructure.Jobs
{
    public class LabTestNotificationJob : ILabTestNotificationJob
    {
        private readonly ApplicationDbContext _context;
        private readonly IEmailService _emailService;
        private readonly ILogger<LabTestNotificationJob> _logger;

        public LabTestNotificationJob(
            ApplicationDbContext context,
            IEmailService emailService,
            ILogger<LabTestNotificationJob> logger)
        {
            _context = context;
            _emailService = emailService;
            _logger = logger;
        }

        public async Task SendResultUploadedNotificationAsync(Guid labTestId)
        {
            var labTest= await _context.LabTests.Include(l=>l.Visit)
                .ThenInclude(v=>v.Appointment)
                .ThenInclude(a=>a.Patient)
                .Include(l => l.Visit)
                    .ThenInclude(v => v.Appointment)
                        .ThenInclude(a => a.Doctor)
                            .ThenInclude(d => d.Employee)
                .FirstOrDefaultAsync(l => l.Id == labTestId);
            if (labTest is null)
            {
                _logger.LogWarning("LabTest {LabTestId} not found when attempting to send notification.", labTestId);
                return;
            }
            var appointment = labTest.Visit.Appointment;
            var patientUser= await _context.Users.FirstOrDefaultAsync(u => u.PatientId == appointment.PatientId);
            var doctorEmployeeId = appointment.Doctor.EmployeeId;
            var doctorUser = await _context.Users.FirstOrDefaultAsync(u => u.EmployeeId == doctorEmployeeId);
            var subject = "Lab Test Result Uploaded - Clinic Management System";
            var body = $"The result for the '{labTest.TestType}' test has been uploaded and is now available.";
            if (patientUser?.Email is not null)
            {
                await TrySendAsync(patientUser.Email, subject, body, labTestId);
            }
            if (doctorUser?.Email is not null)
            {
                await TrySendAsync(doctorUser.Email, subject, body, labTestId);
            }

        }
        private async Task TrySendAsync(string email, string subject, string body, Guid labTestId)
        {
            try
            {
                await _emailService.SendEmailAsync(email, subject, body);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to send lab test notification for LabTest {LabTestId} to {Email}", labTestId, email);
            }

        }

    }
}
