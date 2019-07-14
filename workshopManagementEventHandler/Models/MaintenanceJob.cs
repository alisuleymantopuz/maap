using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace workshopManagementEventHandler.Models
{
    public class MaintenanceJob
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public Vehicle Vehicle { get; set; }
        public Customer Customer { get; set; }
        public string Description { get; set; }
        public DateTime? ActualStartTime { get; set; }
        public DateTime? ActualEndTime { get; set; }
        public string Notes { get; set; }
        public DateTime WorkshopPlanningDate { get; set; }
    }
}
