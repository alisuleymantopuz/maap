using System.Collections.Generic;
using System.Threading.Tasks;
using workshopManagementAPI.Domain;

namespace workshopManagementAPI.Repositories
{
    public interface IVehicleRepository
    {
        Task<IEnumerable<Vehicle>> GetVehiclesAsync();
        Task<Vehicle> GetVehicleAsync(string licenseNumber);
    }
}
