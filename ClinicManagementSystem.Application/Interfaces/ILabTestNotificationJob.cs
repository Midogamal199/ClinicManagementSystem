using System;
using System.Threading.Tasks;

namespace ClinicManagementSystem.Application.Interfaces
{
    public interface ILabTestNotificationJob
    {
        Task SendResultUploadedNotificationAsync(Guid labTestId);
    }
}