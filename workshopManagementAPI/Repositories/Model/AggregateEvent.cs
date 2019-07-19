﻿using System;
namespace workshopManagementAPI.Repositories.Model
{
    public class AggregateEvent
    {
        public string Id { get; set; }
        public int Version { get; set; }
        public DateTime Timestamp { get; set; }
        public string MessageType { get; set; }
        public string EventData { get; set; }
    }
}