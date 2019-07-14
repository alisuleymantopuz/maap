﻿using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using infrastructure.messaging;
using workshopManagementAPI.Commands;
using workshopManagementAPI.Domain.Exceptions;
using workshopManagementAPI.Events;

namespace workshopManagementAPI.Domain
{
    public class WorkshopPlanning
    {
        /// <summary>
        /// Indication whether the aggregate is replaying events (true) or not (false).
        /// </summary>
        private bool IsReplaying { get; set; } = false;

        /// <summary>
        /// The date of the planning (aggregate  Id).
        /// </summary>
        public DateTime Date { get; private set; }

        /// <summary>
        /// The list of maintenance-jobs for this day. 
        /// </summary>
        public List<MaintenanceJob> Jobs { get; private set; }

        /// <summary>
        /// The original version of the aggregate after replaying all events in the event-store.
        /// </summary>
        public int Version { get; private set; }

        /// <summary>
        /// The current version after handling any commands.
        /// </summary>
        public int OriginalVersion { get; private set; }

        /// <summary>
        /// Aggregate Id.
        /// </summary>
        public string Id
        {
            get
            {
                return Date.ToString("yyyy-MM-dd");
            }
        }

        public WorkshopPlanning()
        {
            OriginalVersion = 0;
            Version = 0;
        }

        public WorkshopPlanning(IEnumerable<Event> events)
        {
            OriginalVersion = 0;
            Version = 0;
            IsReplaying = true;
            foreach (Event e in events)
            {
                HandleEvent(e);
                OriginalVersion++;
            }
            IsReplaying = false;
        }

        /// <summary>
        /// Creates a new instance of a workshop-planning for the specified date.
        /// </summary>
        /// <param name="date">The date to create the planning for.</param>
        /// <param name="planning">The initialized WorkshopPlanning instance.</param>
        /// <returns>The WorkshopPlanningCreated event.</returns>
        /// <remarks>This implementation makes sure creation of the planning becomes part of the event-stream.</remarks>
        public static IEnumerable<Event> Create(DateTime date, out WorkshopPlanning planning)
        {
            planning = new WorkshopPlanning();
            WorkshopPlanningCreated e = new WorkshopPlanningCreated(Guid.NewGuid(), date);
            return planning.HandleEvent(e);
        }

        public IEnumerable<Event> PlanMaintenanceJob(PlanMaintenanceJob command)
        {
            // check business rules
            this.PlannedMaintenanceJobShouldFallWithinOneBusinessDay(command);
            this.NumberOfParallelMaintenanceJobsMustNotExceedAvailableWorkStations(command);
            this.NumberOfParallelMaintenanceJobsOnAVehicleIsOne(command);

            // handle event
            MaintenanceJobPlanned e = Mapper.Map<MaintenanceJobPlanned>(command);
            return HandleEvent(e);
        }

        public IEnumerable<Event> FinishMaintenanceJob(FinishMaintenanceJob command)
        {
            // find job
            MaintenanceJob job = Jobs.FirstOrDefault(j => j.Id == command.JobId);
            if (job == null)
            {
                throw new MaintenanceJobNotFoundException($"Maintenance job with id {command.JobId} found.");
            }

            // check business rules
            job.FinishedMaintenanceJobCanNotBeFinished();

            // handle event
            MaintenanceJobFinished e = Mapper.Map<MaintenanceJobFinished>(command);
            return HandleEvent(e);
        }

        /// <summary>
        /// Handles an event and updates the aggregate version.
        /// </summary>
        /// <param name="e">The event to handle.</param>
        private IEnumerable<Event> HandleEvent(dynamic e)
        {
            IEnumerable<Event> events = Handle(e);
            Version += events.Count();
            return events;
        }

        private IEnumerable<Event> Handle(WorkshopPlanningCreated e)
        {
            Jobs = new List<MaintenanceJob>();
            Date = e.Date.Date;
            return new Event[] { e };
        }

        private IEnumerable<Event> Handle(MaintenanceJobPlanned e)
        {
            MaintenanceJob job = new MaintenanceJob();
            Customer customer = new Customer(e.CustomerInfo.Id, e.CustomerInfo.Name, e.CustomerInfo.TelephoneNumber);
            Vehicle vehicle = new Vehicle(e.VehicleInfo.LicenseNumber, e.VehicleInfo.Brand, e.VehicleInfo.Type, customer.CustomerId);
            job.Plan(e.JobId, e.StartTime, e.EndTime, vehicle, customer, e.Description);
            Jobs.Add(job);
            return new Event[] { e };
        }

        private IEnumerable<Event> Handle(MaintenanceJobFinished e)
        {
            MaintenanceJob job = Jobs.FirstOrDefault(j => j.Id == e.JobId);
            job.Finish(e.StartTime, e.EndTime, e.Notes);
            return new Event[] { e };
        }
    }
}
