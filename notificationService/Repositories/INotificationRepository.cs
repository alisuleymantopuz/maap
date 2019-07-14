using System;
using System.Threading.Tasks;
using notificationService.Models;
using System.Collections.Generic;

namespace notificationService.Repositories
{
    public interface INotificationRepository
    {
        Task RegisterCustomerAsync(Customer customer);
        Task RegisterMaintenanceJobAsync(MaintenanceJob job);
        Task<IEnumerable<MaintenanceJob>> GetMaintenanceJobsForTodayAsync(DateTime date);
        Task<Customer> GetCustomerAsync(string customerId);
        Task RemoveMaintenanceJobsAsync(IEnumerable<string> jobIds);
    }
}
