﻿using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using customerManagementAPI.DataAccess;

namespace customerManagementAPI.Migrations
{
    [DbContext(typeof(CustomerManagementDBContext))]
    partial class CustomerManagementDBContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
            modelBuilder
                .HasAnnotation("ProductVersion", "1.1.1");

            modelBuilder.Entity("customerManagementAPI.Models.Customer", b =>
                {
                    b.Property<string>("CustomerId")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Address");

                    b.Property<string>("City");

                    b.Property<string>("EmailAddress");

                    b.Property<string>("Name");

                    b.Property<string>("PostalCode");

                    b.Property<string>("TelephoneNumber");

                    b.HasKey("CustomerId");

                    b.ToTable("Customer");
                });
        }
    }
}
