using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ClinicManagementSystem.Application.Common;
using ClinicManagementSystem.Domain.Interfaces;
using ClinicManagementSystem.Infrastructure.Persistence;

namespace ClinicManagementSystem.Infrastructure.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _context;
        private readonly Dictionary<Type, object> _repositories = new();
        private IDoctorRepository? _doctorRepository;
        private IVisitRepository? _visitRepository;
        private IAppointmentRepository? _appointmentRepository;
        private IEmployeeRepository? _employeeRepository;
        private IDiagnosisRepository? _diagnosisRepository;
        private IPrescriptionRepository? _prescriptionRepository;
        private IAttendanceRepository? _attendanceRepository;
        private ILeaveRequestRepository? _leaveRequestRepository;
        private IDepartmentRepository? _departmentRepository;
        private IInvoiceRepository? _invoiceRepository;
        public IInvoiceRepository InvoiceRepository =>
    _invoiceRepository ??= new InvoiceRepository(_context);
        public UnitOfWork(ApplicationDbContext context)
        {
            _context = context;
        }

        public IDoctorRepository DoctorRepository => _doctorRepository ??= new DoctorRepository(_context);

        public IVisitRepository VisitRepository => _visitRepository ??= new VisitRepository(_context);
        public IAppointmentRepository AppointmentRepository =>
    _appointmentRepository ??= new AppointmentRepository(_context);
        public IEmployeeRepository EmployeeRepository =>
    _employeeRepository ??= new EmployeeRepository(_context);
        public IDiagnosisRepository DiagnosisRepository =>
    _diagnosisRepository ??= new DiagnosisRepository(_context);

        public IPrescriptionRepository PrescriptionRepository =>
            _prescriptionRepository ??= new PrescriptionRepository(_context);
        public IAttendanceRepository AttendanceRepository =>
    _attendanceRepository ??= new AttendanceRepository(_context);
        public ILeaveRequestRepository LeaveRequestRepository =>
    _leaveRequestRepository ??= new LeaveRequestRepository(_context);
        public IDepartmentRepository DepartmentRepository =>
    _departmentRepository ??= new DepartmentRepository(_context);

        public void Dispose()
        {
          _context.Dispose(); 
        }

        public IGenericRepository<T> Repository<T>() where T : BaseEntity
        {
            if (_repositories.ContainsKey(typeof(T)))
            {
                return (IGenericRepository<T>)_repositories[typeof(T)];
            }
            var repository = new GenericRepository<T>(_context);
            _repositories.Add(typeof(T), repository);
            return repository;

        }

        public async Task<int> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync();
        }
    }
}
