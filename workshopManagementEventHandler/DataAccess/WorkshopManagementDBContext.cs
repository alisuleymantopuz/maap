using Microsoft.EntityFrameworkCore;
using workshopManagementEventHandler.Models;

namespace workshopManagementEventHandler.DataAccess
{
    public class WorkshopManagementDBContext : DbContext
    {
        public WorkshopManagementDBContext()
        { }

        public WorkshopManagementDBContext(DbContextOptions<WorkshopManagementDBContext> options) : base(options)
        { }

        public DbSet<Vehicle> Vehicles { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<MaintenanceJob> MaintenanceJobs { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Vehicle>().HasKey(entity => entity.LicenseNumber);
            modelBuilder.Entity<Vehicle>().ToTable("Vehicle");

            modelBuilder.Entity<Customer>().HasKey(entity => entity.CustomerId);
            modelBuilder.Entity<Customer>().ToTable("Customer");

            modelBuilder.Entity<MaintenanceJob>().HasKey(entity => entity.Id);
            modelBuilder.Entity<MaintenanceJob>().ToTable("MaintenanceJob");

            base.OnModelCreating(modelBuilder);
        }
    }
}
