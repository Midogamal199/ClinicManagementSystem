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
    public class AppointmentReminderJob : IAppointmentReminderJob
    {
        private readonly ApplicationDbContext _context;
        private readonly IEmailService _emailService;
        private readonly ILogger<AppointmentReminderJob> _logger;

        public AppointmentReminderJob(
           ApplicationDbContext context,
           IEmailService emailService,
           ILogger<AppointmentReminderJob> logger)
        {
            _context = context;
            _emailService = emailService;
            _logger = logger;
        }

        public async Task SendUpcomingAppointmentRemindersAsync()
        {
            var windowStart = DateTime.Now.AddHours(24);
            var windowEnd = DateTime.Now.AddHours(25);
            var upcomingAppointments = await _context.Appointments
                .Include(a => a.Patient)
                .Where(a =>a.ScheduledAt>=windowStart && a.ScheduledAt < windowEnd).ToListAsync();
            foreach(var appointment in upcomingAppointments)
            {
                var patientUser= await _context.Users.FirstOrDefaultAsync(u => u.PatientId== appointment.PatientId);
                if (patientUser?.Email is null)
                {
                    continue;
                }
                try
                {
                    await _emailService.SendEmailAsync(patientUser.Email, "Appointment Reminder - Clinic Management System",
                        $"Hello {appointment.Patient.FullName},\n\n" +
                        $"This is a reminder that you have an appointment scheduled for " +
                        $"{appointment.ScheduledAt:dddd, dd MMMM yyyy 'at' hh:mm tt}.\n\n" +
                        $"Please arrive 10 minutes early.");
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Failed to send appointment reminder for Appointment {AppointmentId}", appointment.Id);

                }
            }
            _logger.LogInformation("Appointment reminder job completed. {Count} reminders processed.", upcomingAppointments.Count);

        }
    }
}
