using System;
using AutoMapper;
using vehicleManagementAPI.Commands;
using vehicleManagementAPI.Events;
using vehicleManagementAPI.Models;

namespace vehicleManagementAPI
{

    public static class MapperConfiguration
    {
        public static void SetupAutoMapper()
        {
            // setup automapper
            var cfg = new AutoMapper.Configuration.MapperConfigurationExpression();
            cfg.CreateMap<RegisterVehicle, Vehicle>();
            cfg.CreateMap<RegisterVehicle, VehicleRegistered>()
                .ForCtorParam("messageId", opt => opt.MapFrom(c => Guid.NewGuid()));
            Mapper.Initialize(cfg);
        }
    }
}