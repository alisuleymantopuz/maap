using System.Collections.Generic;
using System.Threading.Tasks;
using workshopManagementAPI.Domain;

namespace workshopManagementAPI.Repositories
{
    public interface ICustomerRepository
    {
        Task<IEnumerable<Customer>> GetCustomersAsync();
        Task<Customer> GetCustomerAsync(string customerId);

    }
}
