using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ClinicManagementSystem.Domain.Entities;
using ClinicManagementSystem.Domain.Enums;
using ClinicManagementSystem.Domain.Interfaces;
using ClinicManagementSystem.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace ClinicManagementSystem.Infrastructure.Repositories
{
    public class AppointmentRepository : GenericRepository<Appointment>, IAppointmentRepository
    {
        private readonly ApplicationDbContext _context;
        public AppointmentRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<Appointment?> GetByIdWithDetailsAsync(Guid id)
        {
            return await _context.Appointments
                .Include(a => a.Patient)
                .Include(a => a.Doctor)
                    .ThenInclude(d => d.Employee)
                .FirstOrDefaultAsync(a => a.Id == id);
        }

        public async Task<(IEnumerable<Appointment> Items, int TotalCount)> GetPagedWithDetailsAsync(int pageNumber, int pageSize, Guid? doctorId, Guid? patientId, AppointmentStatus? status)
        {
            IQueryable<Appointment> query = _context.Appointments
                .Include(a => a.Patient)
                .Include(a => a.Doctor)
                    .ThenInclude(d => d.Employee);
           if(doctorId.HasValue)
            {
                query = query.Where(a => a.DoctorId == doctorId.Value);
            }
            if (patientId.HasValue)
            {
                query = query.Where(a => a.PatientId == patientId.Value);
            }
            if (status.HasValue)
            {
                query = query.Where(a => a.Status == status.Value);
            }
            var totalCount = await query.CountAsync();
            var items = await query
               .OrderBy(a => a.ScheduledAt)
               .Skip((pageNumber - 1) * pageSize)
               .Take(pageSize)
               .ToListAsync();
            return (items, totalCount);


        }

        public async Task<bool> HasConflictAsync(Guid doctorId, DateTime scheduledAt, Guid? excludeAppointmentId = null)
        {
            var query = _context.Appointments.Where(a =>
               a.DoctorId == doctorId &&
               a.ScheduledAt == scheduledAt &&
               a.Status != AppointmentStatus.Cancelled);

            if (excludeAppointmentId.HasValue)
            {
                query = query.Where(a => a.Id != excludeAppointmentId.Value);
            }

            return await query.AnyAsync();
        }
    }
}
