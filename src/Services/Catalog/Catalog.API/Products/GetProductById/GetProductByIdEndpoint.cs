
namespace Catalog.API.Products.GetProductById
{

    public record GetProductByIdResonse(Product Product);
    public class GetProductByIdEndpoint : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapGet("/products/{id}" , async(Guid id , ISender sender , IMapper mapper )=>
            {
                var result = await sender.Send(new GetProductByIdQuery(id));
                var resonse = mapper.Map<GetProductByIdResonse>(result);
                return Results.Ok(resonse);
            }).WithName("GetProductById")
            .Produces<GetProductByIdResonse>(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .WithSummary("Get product by id")
            .WithDescription("Get product by id from the catalog");
        }
    }
}
