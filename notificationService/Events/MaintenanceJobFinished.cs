using System;
using infrastructure.messaging;

namespace notificationService.Events
{
    public class MaintenanceJobFinished : Event
    {
        public readonly string JobId;

        public MaintenanceJobFinished(Guid messageId, string jobId) :
            base(messageId)
        {
            JobId = jobId;
        }
    }
}
