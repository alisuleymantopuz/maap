using System.ComponentModel.DataAnnotations.Schema;

namespace workshopManagementEventHandler.Models
{
    public class Customer
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string CustomerId { get; set; }
        public string Name { get; set; }
        public string TelephoneNumber { get; set; }
    }
}
