using System.Net.Mail;
using System.Threading.Tasks;

namespace invoiceService.CommunicationChannels
{
    public interface IEmailCommunicator
    {
        Task SendEmailAsync(MailMessage message);
    }
}
