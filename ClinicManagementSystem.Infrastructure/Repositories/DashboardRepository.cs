using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ClinicManagementSystem.Domain.DTOs.Dashboard;
using ClinicManagementSystem.Domain.Enums;
using ClinicManagementSystem.Domain.Interfaces;
using ClinicManagementSystem.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace ClinicManagementSystem.Infrastructure.Repositories
{
    public class DashboardRepository : IDashboardRepository
    {
        private readonly ApplicationDbContext _context;

        public DashboardRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<int> GetCurrentlyPresentCountAsync()
        {
            var today = DateTime.UtcNow.Date;
            var tomorrow = today.AddDays(1);
            return await _context.Attendances
               .CountAsync(a => a.CheckIn >= today && a.CheckIn < tomorrow && a.CheckOut == null);

        }

        public async Task<List<DepartmentDistributionDto>> GetDepartmentDistributionAsync()
        {
            return await _context.Employees
                .GroupBy(e => e.Department.Name)
                .Select(g => new DepartmentDistributionDto
                {
                    DepartmentName = g.Key,
                    EmployeeCount = g.Count()
                }).OrderByDescending(x => x.EmployeeCount)
                .ToListAsync();
        }

        public async Task<List<DiagnosisTrendItemDto>> GetDiagnosisTrendsAsync(DateTime fromDate, int top = 10)
        {
            return await _context.Diagnoses
                .Where(d => d.CreatedAt >= fromDate)
                .GroupBy(d=>d.Description)
                .Select(g => new DiagnosisTrendItemDto
                {
                    Description = g.Key,
                    Count = g.Count()
                })
                .OrderByDescending(x => x.Count)
                .Take(top)
                .ToListAsync();
        }

        public async Task<List<FrequentVisitorDto>> GetFrequentVisitorsAsync(int top = 10)
        {
            return await _context.Visits
                .GroupBy(v => new {v.Appointment.PatientId, v.Appointment.Patient.FullName})
                 .Select(g => new FrequentVisitorDto
                 {
                     PatientId = g.Key.PatientId,
                     PatientFullName = g.Key.FullName,
                     VisitCount = g.Count()
                 })
                 .OrderByDescending(x => x.VisitCount)
                    .Take(top)
                    .ToListAsync();
        }

        public async Task<List<GenderDistributionItemDto>> GetGenderDistributionAsync()
        {
            return await _context.Patients
              .GroupBy(p => p.Gender)
              .Select(g => new GenderDistributionItemDto
              {
                  Gender = g.Key.ToString(),
                  Count = g.Count()
              })
              .ToListAsync();
        }

        public async Task<int> GetPendingLeaveRequestsCountAsync()
        {
            return await _context.LeaveRequests
                            .CountAsync(l => l.Status == LeaveStatus.Pending);
        }

        public Task<List<RevenueTrendItemDto>> GetRevenueTrendAsync(DateTime fromDate)
        {
            throw new NotImplementedException();
        }

        public async Task<List<SpecialtyDistributionItemDto>> GetSpecialtyDistributionAsync()
        {
            return await _context.Visits
                .SelectMany(v => v.Appointment.Doctor.Specialties.Select(s=>s.Name))
                .GroupBy(name => name) 
                .Select(g => new SpecialtyDistributionItemDto
                {
                    SpecialtyName = g.Key,
                    VisitCount = g.Count()
                })
                .OrderByDescending(x => x.VisitCount)
                .ToListAsync();
        }

        public async Task<int> GetTodayAppointmentsCountAsync()
        {
            var today = DateTime.UtcNow.Date;
            var tomorrow = today.AddDays(1);
            return await _context.Appointments
                .CountAsync(a => a.ScheduledAt >= today && a.ScheduledAt < tomorrow);
        
        }

        public async Task<decimal> GetTodayRevenueAsync()
        {
            var today = DateTime.UtcNow.Date;
            var tomorrow = today.AddDays(1);
            return await _context.Payments
                .Where(p => p.CreatedAt >= today && p.CreatedAt < tomorrow)
                .SumAsync(p => (decimal?)p.Amount) ?? 0m;
        }

        public async Task<int> GetTodayVisitsCountAsync()
        {
            var today = DateTime.UtcNow.Date;
            var tomorrow = today.AddDays(1);    
            return await _context.Visits
                .CountAsync(v => v.VisitDate >= today && v.VisitDate < tomorrow);

        }

        public async Task<List<TopVisitedDoctorDto>> GetTopVisitedDoctorsAsync(int top = 5)
        {
            return await _context.Visits
                .GroupBy(v => new { v.Appointment.DoctorId, v.Appointment.Doctor.Employee.FullName })
                 .Select(g => new TopVisitedDoctorDto
                 {
                     DoctorId = g.Key.DoctorId,
                     DoctorFullName = g.Key.FullName,
                     VisitCount = g.Count()
                 })
                .OrderByDescending(x => x.VisitCount)
                .Take(top)
                .ToListAsync();
        }
    }
}
