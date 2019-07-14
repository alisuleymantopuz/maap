using System;
using infrastructure.messaging;

namespace invoiceService.Events
{
    public class DayHasPassed : Event
    {
        public DayHasPassed(Guid messageId) : base(messageId)
        {
        }
    }
}
