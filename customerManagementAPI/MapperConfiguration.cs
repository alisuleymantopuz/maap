using System;
using AutoMapper;
using customerManagementAPI.Commands;
using customerManagementAPI.Events;
using customerManagementAPI.Models;

namespace customerManagementAPI
{
    public static class MapperConfiguration
    {
        public static void SetupAutoMapper()
        {
            // setup automapper
            var cfg = new AutoMapper.Configuration.MapperConfigurationExpression();
            cfg.CreateMap<RegisterCustomer, Customer>();
            cfg.CreateMap<Customer, RegisterCustomer>().ForCtorParam("messageId", opt => opt.MapFrom(c => Guid.NewGuid()));
            cfg.CreateMap<RegisterCustomer, CustomerRegistered>().ForCtorParam("messageId", opt => opt.MapFrom(c => Guid.NewGuid()));

            Mapper.Initialize(cfg);
        }
    }
}
