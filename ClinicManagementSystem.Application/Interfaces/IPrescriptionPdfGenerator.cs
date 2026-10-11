using ClinicManagementSystem.Application.DTOs.Prescriptions;

namespace ClinicManagementSystem.Application.Interfaces
{
    public interface IPrescriptionPdfGenerator
    {
        byte[] Generate(PrescriptionPdfModel model);
    }
}