using System;
using infrastructure.messaging;

namespace workshopManagementAPI.Commands
{
    public class RegisterPlanning : Command
    {
        public readonly DateTime PlanningDate;

        public RegisterPlanning(Guid messageId, DateTime planningDate) : base(messageId)
        {
            PlanningDate = planningDate;
        }
    }
}
