using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ClinicManagementSystem.Application.Common;

namespace ClinicManagementSystem.Domain.Interfaces
{
    public interface IUnitOfWork:IDisposable
    {
        IGenericRepository<T> Repository<T>() where T : BaseEntity;
        IDoctorRepository DoctorRepository { get; }
        IVisitRepository VisitRepository { get; }
        IAppointmentRepository AppointmentRepository { get; }
        IEmployeeRepository EmployeeRepository { get; }
        IDiagnosisRepository DiagnosisRepository { get; }
        IPrescriptionRepository PrescriptionRepository { get; }
        IAttendanceRepository AttendanceRepository { get; }
        ILeaveRequestRepository LeaveRequestRepository { get; }
       
        IDepartmentRepository DepartmentRepository { get; }
        Task<int> SaveChangesAsync();

    }
}
