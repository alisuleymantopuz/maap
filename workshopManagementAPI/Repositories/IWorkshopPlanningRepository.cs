using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using infrastructure.messaging;
using workshopManagementAPI.Domain;

namespace workshopManagementAPI.Repositories
{
    public interface IWorkshopPlanningRepository
    {
        void EnsureDatabase();
        Task<WorkshopPlanning> GetWorkshopPlanningAsync(DateTime date);
        Task SaveWorkshopPlanningAsync(string planningId, int originalVersion, int newVersion, IEnumerable<Event> newEvents);
    }
}
