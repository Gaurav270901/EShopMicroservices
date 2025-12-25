using Ordering.Application.Orders.Commands.DeleteOrder;
using Ordering.Domain.ValueObjects;

namespace Ordering.API.Endpoints
{
    public record DeleteOrderResponse(bool IsSuccess);
    public class DeleteOrder : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapDelete("order/{id}", async (Guid id, ISender sender) =>
            {
                var command = id.Adapt<DeleteOrderCommand>();
                var result =  await sender.Send(command);

                var response = result.Adapt<DeleteOrderResponse>();
                return Results.Ok(response);
            }).WithName("DeleteOrder")
        .Produces<DeleteOrderResponse>(StatusCodes.Status200OK)
        .ProducesProblem(StatusCodes.Status400BadRequest)
        .ProducesProblem(StatusCodes.Status404NotFound)
        .WithSummary("Delete Order")
        .WithDescription("Delete Order"); ;
        }
    }
}
