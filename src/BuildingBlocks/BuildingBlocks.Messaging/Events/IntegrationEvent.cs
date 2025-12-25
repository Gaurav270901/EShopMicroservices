
namespace BuildingBlocks.Messaging.Events
{
    /// <summary>
    // / Base Integration Event Class
    // use of this class is to create events that will be published to a message broker
    // and consumed by other services in a microservices architecture.
    // / It contains common properties that are typically included in integration events,
    // such as a unique identifier, timestamp, and event type.
    public record IntegrationEvent
    {
        public Guid Id => Guid.NewGuid();
        public DateTime OccuredOn => DateTime.UtcNow;
        public string EventType => GetType().AssemblyQualifiedName;
    }
}
