using WebApp.Models;
using System.Collections.Generic;

namespace WebApp.ViewModels
{
    public class CustomerManagementViewModel
    {
        public IEnumerable<Customer> Customers { get; set; }
    }
}
