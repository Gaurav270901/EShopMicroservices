
namespace Ordering.Domain.Models
{

    public class Order : Aggregate<OrderId>
    {
        private readonly List<OrderItem> _orderItems = new();
        public IReadOnlyList<OrderItem> OrderItems => _orderItems.AsReadOnly();
        public CustomerId CustomerId { get; private set; } = default;
        public OrderName OrderName { get; private set; } = default!;
        public Address ShippingAddress { get; private set; } = default!;
        public Address BillingAddress {  get; private set; } = default!;
        public Payment Payment { get; private set; } = default!;
        public OrderStatus Status { get; private set; } = OrderStatus.Pending;
        public decimal TotalPrice
        {
            get=>OrderItems.Sum(x => x.Price * x.Quantity);
            private set { }
        }
        public static Order Create(OrderId id, CustomerId customerId, OrderName orderName, Address shippingAddress, Address billingAddress, Payment payment)
        {
            var order = new Order
            {
                Id = id,
                CustomerId = customerId,
                OrderName = orderName,
                ShippingAddress = shippingAddress,
                BillingAddress = billingAddress,
                Payment = payment,
                Status = OrderStatus.Pending
            };
            //why to add domain event here
            //when an order is created we might want to notify other parts of the system
            // for example to update inventory or send a confirmation email
            // by adding a domain event here we can decouple the order creation logic from these other concerns
            order.AddDomainEvent(new OrderCreatedEvent(order));

            return order;
        }

        public void Update(OrderName orderName, Address shippingAddress, Address billingAddress, Payment payment, OrderStatus status)
        {
            OrderName = orderName;
            ShippingAddress = shippingAddress;
            BillingAddress = billingAddress;
            Payment = payment;
            Status = status;

            AddDomainEvent(new OrderUpdatedEvent(this));
        }

        public void Add(ProductId productId, int quantity, decimal price)
        {
            ArgumentOutOfRangeException.ThrowIfNegativeOrZero(quantity);
            ArgumentOutOfRangeException.ThrowIfNegativeOrZero(price);

            var orderItem = new OrderItem(Id, productId, quantity, price);
            _orderItems.Add(orderItem);
        }

        public void Remove(ProductId productId)
        {
            var orderItem = _orderItems.FirstOrDefault(x => x.ProductId == productId);
            if (orderItem is not null)
            {
                _orderItems.Remove(orderItem);
            }
        }
    }
}
//anemic domain model vs rich domain model entity
//anemic domain : enitities with little or no business login 
//data structure with getter and setters
//but business rules and behaviour are typically implemented outside the entity often in service layes
//order class is anemic as it only contains properties with getters and private setters and no business logic or behaviour

//rich domain model : entities encapsulate both data and behaviour
// they contain business logic and rules related to the entity itself
// rich domain models promote encapsulation and cohesion by keeping related data and behaviour together within the entity
// rich domain model entities often provide methods to manipulate their state and enforce business rules
// example : an order entity might have methods to add or remove items, calculate total price, apply discounts, etc

//anemic model become more complex as business logic grows leading to scattered logic across services making it harder
//to maintain and understand
// rich domain models provide a more cohesive and maintainable approach by encapsulating
// related logic within the entity itself simplifying the overall design of the system