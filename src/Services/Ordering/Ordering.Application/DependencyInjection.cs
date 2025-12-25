using System.Reflection;
using BuildingBlocks.Behavior;
using BuildingBlocks.Behaviors;
using Microsoft.Extensions.DependencyInjection;

namespace Ordering.Application
{
    public static class DependencyInjection 
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            services.AddMediatR(cfg =>
            {
                cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly());
                cfg.AddOpenBehavior(typeof(ValidationBehavior<,>));
                cfg.AddOpenBehavior(typeof(LoggingBehavior<,>));
            });
            return services;
        }
    }
}
// what is event sourcing pattern 
// Event Sourcing is a design pattern in which state changes to an application are stored
// as a sequence of events.
// Instead of storing just the current state of an entity, every change (event) that
// occurs to that entity is recorded in an append-only log.
// This allows for a complete history of changes, enabling features like auditing,
// temporal queries, and the ability to reconstruct past states of the entity by replaying events.
// In the context of the provided code, the DispatchDomainEventsInterceptor is part of
// an implementation that supports event sourcing by ensuring that domain events
// are dispatched before changes are saved to the database.
// in simple words 
// Event Sourcing is a way to keep track of all the changes made to data
// by storing each change as a separate event, allowing you to see the full history

//eventual consistency principle
// Eventual consistency is a consistency model used in distributed systems
// where updates to a data item will eventually propagate to all replicas,
// but not necessarily immediately.
// In an eventually consistent system, it is acceptable for different nodes
// to have different versions of the same data for a period of time.
// The system guarantees that, given enough time and no new updates,
// all replicas will converge to the same state.

// strict consistency
// Strict consistency, also known as strong consistency, is a consistency model
// where all operations on a data item are immediately visible to all nodes in
// a distributed system.