using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ClinicManagementSystem.Application.DTOs.Search;
using ClinicManagementSystem.Application.Interfaces;
using ClinicManagementSystem.Domain.Entities;
using ClinicManagementSystem.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace ClinicManagementSystem.Infrastructure.Repositories
{
    public class SearchRepository : ISearchRepository
    {
        private readonly ApplicationDbContext _context;

        public SearchRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<List<SearchHitDto>> SearchEmployeesAsync(string term, int limit)
        {
            var rows = await _context.Employees
                .Where(e => e.FullName.Contains(term)
                         || e.PhoneNumber.Contains(term)
                         || e.Position.Contains(term)
                         || e.Department.Name.Contains(term))
                .OrderBy(e => e.FullName)
                .Take(limit)
                .Select(e => new
                {
                    e.Id,
                    e.FullName,
                    e.Position,
                    DepartmentName = e.Department.Name
                })
                .ToListAsync();
            return rows.Select(r => new SearchHitDto
            {
                Id = r.Id,
                Title = r.FullName,
                Subtitle = $"{r.Position} - {r.DepartmentName}"
            }).ToList();


        }

        public async Task<List<SearchHitDto>> SearchLabTestsAsync(string term, int limit, Guid? doctorId)
        {

            var query = _context.LabTests.AsQueryable();
            if (doctorId.HasValue)
            {
                var scopeDoctorId = doctorId.Value;
                query = query.Where(l => l.Visit.Appointment.DoctorId == scopeDoctorId);
            }

            var rows = await query
                .Where(l => l.TestType.Contains(term)
                         || l.Visit.Appointment.Patient.FullName.Contains(term))
                .OrderByDescending(l => l.CreatedAt)
                .Take(limit)
                .Select(l => new
                {
                    l.Id,
                    l.TestType,
                    l.Status,
                    PatientName = l.Visit.Appointment.Patient.FullName
                })
                .ToListAsync();
            return rows.Select(r => new SearchHitDto
            {
                Id = r.Id,
                Title = r.TestType,
                Subtitle = $"{r.PatientName} - {r.Status}"
            }).ToList();
        }

        public async Task<List<SearchHitDto>> SearchPatientsAsync(string term, int limit, bool includeDiagnosisMatches, Guid? restrictDiagnosesToDoctorId)
        {
            IQueryable<Patient> query;
            if (includeDiagnosisMatches)
            {
                var diagnosisQuery = _context.Diagnoses.Where(d=> d.Description.Contains(term)||d.IcdCode.Contains(term));
                if (restrictDiagnosesToDoctorId.HasValue)
                {
                    var doctorId = restrictDiagnosesToDoctorId.Value;
                    diagnosisQuery = diagnosisQuery
                    .Where(d => d.Visit.Appointment.DoctorId == doctorId);
                }
                var patientIdsByDiagnosis = diagnosisQuery.Select(d => d.Visit.Appointment.PatientId);
                query = _context.Patients.Where(p =>
                 p.FullName.Contains(term)
                 || p.PhoneNumber.Contains(term)
                 || patientIdsByDiagnosis.Contains(p.Id));
            }
            else {
                query = _context.Patients
                    .Where(p => p.FullName.Contains(term) || p.PhoneNumber.Contains(term));
            }
            var rows = await query
                .OrderBy(p => p.FullName)
                .Take(limit)
                .Select(p => new { p.Id, p.FullName, p.PhoneNumber })
                .ToListAsync();
            return rows.Select(r => new SearchHitDto
            {
                Id = r.Id,
                Title = r.FullName,
                Subtitle = r.PhoneNumber
            }).ToList();
        }

        public async Task<List<SearchHitDto>> SearchPrescriptionsAsync(string term, int limit, Guid? doctorId)
        {
            var query = _context.Prescriptions.AsQueryable();
            if (doctorId.HasValue)
            {
                var scopeDoctorId = doctorId.Value;
                query = query.Where(p => p.Visit.Appointment.DoctorId == scopeDoctorId);

            }
            var rows = await query
            .Where(p => p.Items.Any(i => i.MedicineName.Contains(term))
             || p.Visit.Appointment.Patient.FullName.Contains(term))
            .OrderByDescending(p => p.IssuedAt)
            .Take(limit)
             .Select(p => new
             {
                p.Id,
                p.IssuedAt,
                PatientName = p.Visit.Appointment.Patient.FullName,
                Medicines = p.Items.Select(i => i.MedicineName).ToList()
               })
                .ToListAsync();
            return rows.Select(r => new SearchHitDto
            {
                Id = r.Id,
                Title = r.PatientName,
                Subtitle = $"{r.IssuedAt:yyyy-MM-dd} - {string.Join(", ", r.Medicines)}"
            }).ToList();

        }
    }
}
