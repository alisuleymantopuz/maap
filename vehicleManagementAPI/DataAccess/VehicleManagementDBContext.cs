using System;
using Microsoft.EntityFrameworkCore;
using Polly;
using vehicleManagementAPI.Models;

namespace vehicleManagementAPI.DataAccess
{
    public class VehicleManagementDBContext : DbContext
    {
        public VehicleManagementDBContext(DbContextOptions<VehicleManagementDBContext> options) : base(options)
        {
        }

        public DbSet<Vehicle> Vehicles { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Vehicle>().HasKey(m => m.LicenseNumber);
            modelBuilder.Entity<Vehicle>().ToTable("Vehicle");
            base.OnModelCreating(modelBuilder);
        }

        public void MigrateDB()
        {
            Policy
                .Handle<Exception>()
                .WaitAndRetry(10, r => TimeSpan.FromSeconds(10))
                .Execute(() => Database.Migrate());
        }
    }
}
