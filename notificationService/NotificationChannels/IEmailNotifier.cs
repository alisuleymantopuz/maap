using System.Threading.Tasks;

namespace notificationService.NotificationChannels
{
    public interface IEmailNotifier
    {
        Task SendEmailAsync(string to, string from, string subject, string body);
    }
}
