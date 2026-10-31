using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ClinicManagementSystem.Application.Interfaces;
using ClinicManagementSystem.Infrastructure.Identity;
using Microsoft.Extensions.Logging;

namespace ClinicManagementSystem.Infrastructure.Jobs
{
    public class ScheduledReportsJob : IScheduledReportsJob
    {
        private readonly IRevenueReportService _reportService;
        private readonly IIdentityService _identityService;
        private readonly IEmailService _emailService;
        private readonly ILogger<ScheduledReportsJob> _logger;

        public ScheduledReportsJob(
            IRevenueReportService reportService,
            IIdentityService identityService,
            IEmailService emailService,
            ILogger<ScheduledReportsJob> logger)
        {
            _reportService = reportService;
            _identityService = identityService;
            _emailService = emailService;
            _logger = logger;
        }
        public Task SendMonthlyRevenueReportAsync()
        {
            var today = DateTime.UtcNow.Date;
            var firstOfThisMonth = new DateTime(today.Year, today.Month, 1);
            var start = firstOfThisMonth.AddMonths(-1);
            var end = firstOfThisMonth.AddDays(-1);
            return SendRevenueReportAsync("Monthly", start, end);

        }

        public Task SendWeeklyRevenueReportAsync()
        {
            var end = DateTime.UtcNow.Date.AddDays(-1); 
            var start = end.AddDays(-6);

            return SendRevenueReportAsync("Weekly", start, end);

        }

        private async Task SendRevenueReportAsync(string label, DateTime start, DateTime end)
        {
            var recipients = await _identityService.GetEmailsByRoleAsync(Roles.Admin);

            if (recipients.Count == 0)
            {
                _logger.LogWarning("{Label} revenue report skipped: no admin accounts with an email.", label);
                return;
            }
            var model = await _reportService.BuildModelAsync(start, end);
            var file= _reportService.ToExcelFile(model, $"revenue-{start:yyyy-MM-dd}-to-{end:yyyy-MM-dd}.xlsx");
            var attachment = new EmailAttachment
            {
                FileName = file.FileName,
                ContentType = file.ContentType,
                Content = file.Content
            };
            var subject = $"{label} revenue report ({start:yyyy-MM-dd} to {end:yyyy-MM-dd}) - Clinic Management System";
            var body =
                "Hello,\n\n" +
                $"Please find attached the {label.ToLowerInvariant()} revenue report " +
                $"for {start:yyyy-MM-dd} to {end:yyyy-MM-dd}.\n\n" +
                $"Total revenue: {model.TotalRevenue:N2} EGP\n" +
                $"Number of payments: {model.PaymentsCount}\n\n" +
                "This is an automated message.";

            var sent = 0;
            foreach (var email in recipients)
            {
                try
                {
                    await _emailService.SendEmailAsync(email, subject, body, new[] { attachment });
                    sent++;
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Failed to send {Label} revenue report to {Email}", label, email);
                }
            }
            _logger.LogInformation(
               "{Label} revenue report ({Start} to {End}) sent to {Sent}/{Total} recipients.",
               label, start, end, sent, recipients.Count);

        }
    }
}
