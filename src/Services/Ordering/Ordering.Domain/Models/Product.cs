
namespace Ordering.Domain.Models
{
    //
    public class Product : Entity<ProductId>
    {
        public string Name { get; private set; } = default!;
        public decimal Price { get; private set; } = default!;
        public static Product Create(ProductId id, string name, decimal price)
        {
            ArgumentException.ThrowIfNullOrWhiteSpace(name);
            ArgumentOutOfRangeException.ThrowIfNegativeOrZero(price);

            var product = new Product
            {
                Id = id,
                Name = name,
                Price = price
            };

            return product;
        }
    }
}
//what is domain events in domain driven design
//Domain events are a way to capture and communicate significant occurrences or
//changes within the domain model. They represent events that have business significance
//and are used to notify other parts of the system about these changes. Domain events help
//to decouple different parts of the system, allowing them to react to changes without
//being tightly coupled to the source of the change.
//For example, when an order is created, a domain event can be raised to notify other parts
//of the system, such as inventory management or notification services,
//about the new order. This allows these components to react accordingly without being
//directly dependent on the order creation logic.

//domain vs integration events
//Domain events are specific to the domain model and represent significant occurrences
//within that domain. They are used to communicate changes within the same bounded context.
//Integration events, on the other hand, are used to communicate between different
//bounded contexts or microservices. They are typically used for cross-cutting concerns
//and to facilitate communication between different parts of a distributed system. 
//Integration events are often published to a message broker or event bus,like RabbitMQ or Kafka,
