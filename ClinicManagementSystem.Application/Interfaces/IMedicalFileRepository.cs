using System;
using System.Threading.Tasks;
using ClinicManagementSystem.Application.DTOs.MedicalFiles;

namespace ClinicManagementSystem.Domain.Interfaces
{
    public interface IMedicalFileRepository
    {
        Task<MedicalFileDto?> GetByPatientIdAsync(Guid patientId);
        Task<bool> DoctorHasAppointmentWithPatientAsync(Guid doctorId, Guid patientId);
    }
}