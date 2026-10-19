using System;
using System.Linq;
using System.Threading.Tasks;
using ClinicManagementSystem.Application.Interfaces;
using ClinicManagementSystem.Domain.Entities;
using ClinicManagementSystem.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace ClinicManagementSystem.Infrastructure.Jobs
{
    public class PrescriptionTreatmentEndJob : IPrescriptionTreatmentEndJob
    {
        private const int LookbackDays = 3;

        private readonly ApplicationDbContext _context;
        private readonly IEmailService _emailService;
        private readonly ILogger<PrescriptionTreatmentEndJob> _logger;

        public PrescriptionTreatmentEndJob(
            ApplicationDbContext context,
            IEmailService emailService,
            ILogger<PrescriptionTreatmentEndJob> logger)
        {
            _context = context;
            _emailService = emailService;
            _logger = logger;
        }

        public async Task SendTreatmentEndAlertsAsync()
        {
            var now = DateTime.UtcNow;
            var today = now.Date;
            var lookbackStart = today.AddDays(-LookbackDays);

            // الفلترة بتاريخ النهاية بتحصل في SQL، فبيتحمّل بس اللي مدته خلصت فعلًا
            var candidates = await _context.PrescriptionItems
                .Include(i => i.Prescription)
                    .ThenInclude(p => p.Visit)
                        .ThenInclude(v => v.Appointment)
                            .ThenInclude(a => a.Patient)
                .Where(i => i.DurationInDays != null
                         && i.EndAlertProcessedAt == null
                         && i.Prescription.IssuedAt.Date.AddDays(i.DurationInDays.Value) <= today)
                .ToListAsync();

            // علاجات خلصت من أكتر من 3 أيام ومتبعتش ليها تنبيه: نقفلها من غير إرسال
            foreach (var stale in candidates.Where(i => GetEndDate(i) < lookbackStart))
            {
                stale.EndAlertProcessedAt = now;
            }

            var due = candidates
                .Where(i => GetEndDate(i) >= lookbackStart)
                .ToList();

            await _context.SaveChangesAsync();

            if (due.Count == 0)
            {
                _logger.LogInformation("Treatment-end alert job completed. No items were due.");
                return;
            }

            // إيميلات كل المرضى المعنيين في استعلام واحد بدل استعلام لكل روشتة
            var patientIds = due
                .Select(i => i.Prescription.Visit.Appointment.PatientId)
                .Distinct()
                .ToList();

            var accounts = await _context.Users
                .Where(u => u.PatientId != null
                         && u.Email != null
                         && patientIds.Contains(u.PatientId.Value))
                .Select(u => new { PatientId = u.PatientId!.Value, Email = u.Email! })
                .ToListAsync();

            var emailByPatientId = accounts
                .GroupBy(a => a.PatientId)
                .ToDictionary(g => g.Key, g => g.First().Email);

            foreach (var group in due.GroupBy(i => i.PrescriptionId))
            {
                var prescription = group.First().Prescription;
                var patient = prescription.Visit.Appointment.Patient;

                if (!emailByPatientId.TryGetValue(patient.Id, out var email))
                {
                    // مفيش حساب نبعتله: منعلّمهاش، لو سجّل في نافذة الـ 3 أيام هيوصله
                    continue;
                }

                try
                {
                    var medicines = string.Join("\n", group.Select(i =>
                        $"- {i.MedicineName} ({i.Dosage}) for {i.DurationInDays} day(s)"));

                    await _emailService.SendEmailAsync(
                        email,
                        "Your treatment course has ended - Clinic Management System",
                        $"Hello {patient.FullName},\n\n" +
                        $"The treatment course prescribed on {prescription.IssuedAt:yyyy-MM-dd} has reached its planned end:\n" +
                        $"{medicines}\n\n" +
                        "If your symptoms persist or you have any concerns, please contact the clinic or book a follow-up appointment.\n\n" +
                        "This is an automated message.");

                    foreach (var item in group)
                    {
                        item.EndAlertProcessedAt = now;
                    }

                    await _context.SaveChangesAsync();
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex,
                        "Failed to send treatment-end alert for Prescription {PrescriptionId}", prescription.Id);
                }
            }

            _logger.LogInformation(
                "Treatment-end alert job completed. {Count} prescription item(s) were due.", due.Count);
        }

        private static DateTime GetEndDate(PrescriptionItem item)
        {
            return item.Prescription.IssuedAt.Date.AddDays(item.DurationInDays!.Value);
        }
    }
}