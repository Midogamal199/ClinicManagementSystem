using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ClinicManagementSystem.Domain.Entities;
using ClinicManagementSystem.Domain.Enums;

namespace ClinicManagementSystem.Domain.Interfaces
{
    public interface IAppointmentRepository: IGenericRepository<Appointment>
    {
        Task<Appointment?> GetByIdWithDetailsAsync(Guid id);
        Task<(IEnumerable<Appointment> Items, int TotalCount)> GetPagedWithDetailsAsync(
           int pageNumber,
           int pageSize,
           Guid? doctorId,
           Guid? patientId,
           AppointmentStatus? status);
        Task<bool> HasConflictAsync(Guid doctorId, DateTime scheduledAt, Guid? excludeAppointmentId = null);
    }
}
