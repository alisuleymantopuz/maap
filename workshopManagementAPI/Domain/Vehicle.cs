﻿namespace workshopManagementAPI.Domain
{
    public class Vehicle
    {
        public string LicenseNumber { get; private set; }
        public string Brand { get; private set; }
        public string Type { get; private set; }
        public string OwnerId { get; private set; }

        public Vehicle(string licenseNumber, string brand, string type, string ownerId)
        {
            LicenseNumber = licenseNumber;
            Brand = brand;
            Type = type;
            OwnerId = ownerId;
        }
    }
}
