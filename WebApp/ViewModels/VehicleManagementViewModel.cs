using WebApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApp.ViewModels
{
    public class VehicleManagementViewModel
    {
        public IEnumerable<Vehicle> Vehicles { get; set; }
    }
}
