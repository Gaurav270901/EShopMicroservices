
namespace Ordering.Application.Orders.Queries.GetOrdersByName
{
    public class GetOrdersByNameHandler(IApplicationDbContext dbContext) 
        : IQueryHandler<GetOrdersByNameQuery, GetOrdersByNameResult>
    {
        public async Task<GetOrdersByNameResult> Handle(GetOrdersByNameQuery query, CancellationToken cancellationToken)
        {
            // fetch orders by name from the database using the provided dbContext
            // map the fetched orders to OrderDto
            // return the result as GetOrdersByNameResult
            var orders = await dbContext.Orders
                .Include(o => o.OrderItems)
                .AsNoTracking()
                .Where(o => o.OrderName.Value.Contains(query.Name))
                .OrderBy(o => o.OrderName.Value)
                .ToListAsync(cancellationToken);


            //how did orders get this ToOrderDtoList() method?
            // Because of the using Ordering.Application.Extentions; directive at the top
            // which brings in the extension method defined in OrderExtentions class
            // This method converts a list of Order entities to a list of OrderDto objects.
            return new GetOrdersByNameResult(orders.ToOrderDtoList());
        }
    }
}
