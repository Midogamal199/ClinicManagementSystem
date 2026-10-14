using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Azure.Identity;
using ClinicManagementSystem.Application.DTOs.MedicalFiles;
using ClinicManagementSystem.Application.DTOs.Prescriptions;
using ClinicManagementSystem.Domain.Interfaces;
using ClinicManagementSystem.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace ClinicManagementSystem.Infrastructure.Repositories
{
    public class MedicalFileRepository : IMedicalFileRepository
    {
        private readonly ApplicationDbContext _context;

        public MedicalFileRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<bool> DoctorHasAppointmentWithPatientAsync(Guid doctorId, Guid patientId)
        {
            return await _context.Appointments.AnyAsync(a => a.DoctorId == doctorId && a.PatientId == patientId);
        }

        public async Task<MedicalFileDto?> GetByPatientIdAsync(Guid patientId)
        {
            var patient = await _context.Patients
                .Where(p => p.Id == patientId)
                .Select(p => new
                {
                    p.Id,
                    p.FullName,
                    p.DateOfBirth,
                    p.Gender,
                    p.PhoneNumber,
                    p.Address
                }).FirstOrDefaultAsync();
            if (patient is null)
            {
                return null;
            }
            var visits = await _context.Visits
                .Where(v => v.Appointment.PatientId == patientId)
                .OrderByDescending(v => v.VisitDate)
                .Select(v => new MedicalFileVisitDto
                {
                    VisitId = v.Id,
                    VisitDate = v.VisitDate,
                    Notes = v.Notes,
                    DoctorFullName = v.Appointment.Doctor.Employee.FullName
                })
                .ToListAsync();
            var visitIds = visits.Select(v => v.VisitId).ToList();
            var diagnoses = await _context.Diagnoses
                .Where(d => visitIds.Contains(d.VisitId))
                .Select(d => new { d.VisitId, d.Id, d.Description, d.IcdCode })
                .ToListAsync();
            var prescriptions = await _context.Prescriptions
                .Where(p => visitIds.Contains(p.VisitId))
                .Select(p => new {
                    p.VisitId,
                    p.Id,
                    p.IssuedAt,
                    Items = p.Items
                        .Select(i => new PrescriptionItemDto
                        {
                            Id = i.Id,
                            MedicineName = i.MedicineName,
                            Dosage = i.Dosage
                        })
                        .ToList()
                })
                .ToListAsync();
            var labTests = await _context.LabTests
                .Where(l => visitIds.Contains(l.VisitId))
                .Select(l => new
                {
                    l.VisitId,
                    l.Id,
                    l.TestType,
                    l.Status,
                    l.ResultFileUrl,
                    l.ResultFileName,
                    l.UploadedAt
                }).ToListAsync();

            var diagnosesByVisit = diagnoses.ToLookup(d => d.VisitId);
            var prescriptionsByVisit = prescriptions.ToLookup(p => p.VisitId);
            var labTestsByVisit = labTests.ToLookup(l => l.VisitId);
            foreach (var visit in visits) {
                visit.Diagnoses = diagnosesByVisit[visit.VisitId]
                        .Select(d => new MedicalFileDiagnosisDto
                        {
                            Id = d.Id,
                            Description = d.Description,
                            IcdCode = d.IcdCode
                        })
                        .ToList();
                visit.Prescriptions = prescriptionsByVisit[visit.VisitId]
                    .Select(p => new MedicalFilePrescriptionDto
                    {
                        Id = p.Id,
                        IssuedAt = p.IssuedAt,
                        Items = p.Items
                    })
                    .ToList();
                visit.LabTests = labTestsByVisit[visit.VisitId]
                   .Select(l => new MedicalFileLabTestDto
                   {
                       Id = l.Id,
                       TestType = l.TestType,
                       Status = l.Status.ToString(),
                       ResultFileUrl = l.ResultFileUrl is null
                       ? null
                       : $"/api/LabTest/{l.Id}/result-file",
                       ResultFileName = l.ResultFileName,
                       UploadedAt = l.UploadedAt
                   })
                   .ToList();

            }
            return new MedicalFileDto
            {
                PatientId = patient.Id,
                FullName = patient.FullName,
                DateOfBirth = patient.DateOfBirth,
                Gender = patient.Gender.ToString(),
                PhoneNumber = patient.PhoneNumber,
                Address = patient.Address,
                Visits = visits

            };
        }
    } 
}

