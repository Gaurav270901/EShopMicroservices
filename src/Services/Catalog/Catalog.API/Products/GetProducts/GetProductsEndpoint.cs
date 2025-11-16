
using Catalog.API.Products.GetProduct;

namespace Catalog.API.Products.GetProducts
{
    public record GetProductRequest(int? PageNumber = 1 , int? PageSize = 10);
    public record GetProductResponse(IEnumerable<Product> Products);
    public class GetProductsEndpoint : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapGet("/Products", async ([AsParameters] GetProductRequest request , ISender sender, IMapper mapper) =>
            {
                var query = mapper.Map<GetProductsQuery>(request);
                var result = await sender.Send(query);
                var response = mapper.Map<GetProductResponse>(result);
                return Results.Ok(response);
            }).WithName("GetProducts")
            .Produces<GetProductResponse>(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .WithSummary("Get all products")
            .WithDescription("Get all products");
        }
    }
}
