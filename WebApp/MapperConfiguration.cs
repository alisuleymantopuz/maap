using System;
using AutoMapper;
using WebApp.Commands;
using WebApp.Models;
using WebApp.ViewModels;

namespace WebApp
{
    public static class MapperConfiguration
    {
        public static void SetupAutoMapper()
        {
            // setup automapper
            var cfg = new AutoMapper.Configuration.MapperConfigurationExpression();
            cfg.CreateMap<Customer, RegisterCustomer>().ForCtorParam("messageId", opt => opt.MapFrom(c => Guid.NewGuid())).ForCtorParam("customerId", opt => opt.MapFrom(c => Guid.NewGuid()));
            cfg.CreateMap<Vehicle, RegisterVehicle>().ForCtorParam("messageId", opt => opt.MapFrom(c => Guid.NewGuid()));
            cfg.CreateMap<VehicleManagementNewViewModel, RegisterVehicle>().ConvertUsing((vm, rv) => new RegisterVehicle(Guid.NewGuid(), vm.Vehicle.LicenseNumber, vm.Vehicle.Brand, vm.Vehicle.Type, vm.SelectedCustomerId));

            Mapper.Initialize(cfg);
        }
    }
}
