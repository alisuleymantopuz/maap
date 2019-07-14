using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace vehicleManagementAPI.Models
{
    public class Vehicle
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string LicenseNumber { get; set; }
        public string Brand { get; set; }
        public string Type { get; set; }
        public string OwnerId { get; set; }
    }
}
