
using MediatR;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace Ordering.Infrastructure.Data.Interceptors
{
    // This interceptor dispatches domain events before saving changes to the database.
    // what is dispating : It means to send or publish domain events to their respective handlers.
    // This ensures that any side effects or additional processing required by the domain events
    // are executed before the data is persisted.
    // what is interceptors : Interceptors in Entity Framework Core are a way to hook into the
    // lifecycle of database operations.
    // in simple word : Interceptors allow you to run custom logic at specific points during the
    // execution of database commands, such as before or after saving changes.
    public class DispatchDomainEventsInterceptor(IMediator mediator)
    : SaveChangesInterceptor
    {
        // why to use savingchanges and not saveChanges : 
        // SavingChanges is called before the changes are saved to the database,
        // allowing you to perform actions or validations before the actual save operation.
        // On the other hand, SaveChanges is called after the changes have been saved,
        public override InterceptionResult<int> SavingChanges(DbContextEventData eventData, InterceptionResult<int> result)
        {
            DispatchDomainEvents(eventData.Context).GetAwaiter().GetResult();
            return base.SavingChanges(eventData, result);
        }

        public override async ValueTask<InterceptionResult<int>> SavingChangesAsync(DbContextEventData eventData, InterceptionResult<int> result, CancellationToken cancellationToken = default)
        {
            await DispatchDomainEvents(eventData.Context);
            return await base.SavingChangesAsync(eventData, result, cancellationToken);
        }


        //dispatch domain events
        // This method retrieves all entities that implement the IAggregate interface
        // and have domain events.
        // It collects all domain events, clears them from the entities,
        // and then publishes each event using the MediatR mediator.
        // This ensures that all domain events are handled before the changes
        public async Task DispatchDomainEvents(DbContext? context)
        {
            if (context == null) return;

            var aggregates = context.ChangeTracker
                .Entries<IAggregate>()
                .Where(a => a.Entity.DomainEvents.Any())
                .Select(a => a.Entity);

            var domainEvents = aggregates
                .SelectMany(a => a.DomainEvents)
                .ToList();

            aggregates.ToList().ForEach(a => a.ClearDomainEvents());

            foreach (var domainEvent in domainEvents)
                await mediator.Publish(domainEvent);

            //mediator will publish this events to respective handlers
            //ex. OrderCreatedEvent will be published to OrderCreatedEventHandler
        }
    }
}
