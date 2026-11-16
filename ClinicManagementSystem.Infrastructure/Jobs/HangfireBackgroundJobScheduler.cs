using System;
using ClinicManagementSystem.Application.Interfaces;
using Hangfire;

namespace ClinicManagementSystem.Infrastructure.Jobs
{
    public class HangfireBackgroundJobScheduler : IBackgroundJobScheduler
    {
        private readonly IBackgroundJobClient _jobClient;

        public HangfireBackgroundJobScheduler(IBackgroundJobClient jobClient)
        {
            _jobClient = jobClient;
        }

        public void EnqueueLabTestResultNotification(Guid labTestId)
        {
            _jobClient.Enqueue<ILabTestNotificationJob>(
                job => job.SendResultUploadedNotificationAsync(labTestId));
        }
    }
}