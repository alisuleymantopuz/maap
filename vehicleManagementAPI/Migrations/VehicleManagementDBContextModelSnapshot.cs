using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using vehicleManagementAPI.DataAccess;

namespace vehicleManagementAPI.Migrations
{
    [DbContext(typeof(VehicleManagementDBContext))]
    partial class VehicleManagementDBContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
            modelBuilder
                .HasAnnotation("ProductVersion", "1.1.1");

            modelBuilder.Entity("vehicleManagementAPI.Models.Vehicle", b =>
                {
                    b.Property<string>("LicenseNumber")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Brand");

                    b.Property<string>("OwnerId");

                    b.Property<string>("Type");

                    b.HasKey("LicenseNumber");

                    b.ToTable("Vehicle");
                });
        }
    }
}
