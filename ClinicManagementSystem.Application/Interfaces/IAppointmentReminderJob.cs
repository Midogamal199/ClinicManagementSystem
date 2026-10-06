using System.Threading.Tasks;

namespace ClinicManagementSystem.Application.Interfaces
{
    public interface IAppointmentReminderJob
    {
        Task SendUpcomingAppointmentRemindersAsync();
    }
}