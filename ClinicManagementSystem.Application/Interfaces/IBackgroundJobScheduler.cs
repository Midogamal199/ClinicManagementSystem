using System;

namespace ClinicManagementSystem.Application.Interfaces
{
    public interface IBackgroundJobScheduler
    {
        void EnqueueLabTestResultNotification(Guid labTestId);
    }
}