using System.Threading.Tasks;

namespace ClinicManagementSystem.Application.Interfaces
{
    public interface IPrescriptionTreatmentEndJob
    {
        Task SendTreatmentEndAlertsAsync();
    }
}