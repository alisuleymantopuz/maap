using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace infrastructure.messaging
{
    public interface IMessageHandlerCallback
    {
        Task<bool> HandleMessageAsync(string messageType, string message);
    }
}
