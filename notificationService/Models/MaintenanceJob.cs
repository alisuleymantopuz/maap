using System;
namespace notificationService.Models
{
    public class MaintenanceJob
    {
        public string JobId { get; set; }
        public string LicenseNumber { get; set; }
        public string CustomerId { get; set; }
        public DateTime StartTime { get; set; }
        public string Description { get; set; }
    }
}
