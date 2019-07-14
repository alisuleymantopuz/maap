using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using infrastructure.messaging;
using Microsoft.Extensions.DependencyInjection;
using workshopManagementAPI.Commands;
using workshopManagementAPI.Domain;
using workshopManagementAPI.Repositories;

namespace workshopManagementAPI.CommandHandlers
{
    public static class CommandHandlersDIRegistration
    {
        public static void AddCommandHandlers(this IServiceCollection services)
        {
            services.AddTransient<IPlanMaintenanceJobCommandHandler, PlanMaintenanceJobCommandHandler>();
            services.AddTransient<IFinishMaintenanceJobCommandHandler, FinishMaintenanceJobCommandHandler>();
        }
    }

    public interface IFinishMaintenanceJobCommandHandler
    {
        Task<WorkshopPlanning> HandleCommandAsync(DateTime planningDate, FinishMaintenanceJob command);
    }

    public interface IPlanMaintenanceJobCommandHandler
    {
        Task<WorkshopPlanning> HandleCommandAsync(DateTime planningDate, PlanMaintenanceJob command);
    }

    public class PlanMaintenanceJobCommandHandler : IPlanMaintenanceJobCommandHandler
    {
        IMessagePublisher _messagePublisher;
        IWorkshopPlanningRepository _planningRepo;

        public PlanMaintenanceJobCommandHandler(IMessagePublisher messagePublisher, IWorkshopPlanningRepository planningRepo)
        {
            _messagePublisher = messagePublisher;
            _planningRepo = planningRepo;
        }

        public async Task<WorkshopPlanning> HandleCommandAsync(DateTime planningDate, PlanMaintenanceJob command)
        {
            List<Event> events = new List<Event>();

            // get or create workshop-planning
            WorkshopPlanning planning = await _planningRepo.GetWorkshopPlanningAsync(planningDate);
            if (planning == null)
            {
                events.AddRange(WorkshopPlanning.Create(planningDate, out planning));
            }

            // handle command
            events.AddRange(planning.PlanMaintenanceJob(command));

            // persist
            await _planningRepo.SaveWorkshopPlanningAsync(
                planning.Id, planning.OriginalVersion, planning.Version, events);

            // publish event
            foreach (var e in events)
            {
                await _messagePublisher.PublishMessageAsync(e.MessageType, e, "");
            }

            // return result
            return planning;
        }
    }
}
