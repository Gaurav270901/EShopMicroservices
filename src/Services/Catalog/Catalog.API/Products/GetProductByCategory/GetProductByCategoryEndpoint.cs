
using Catalog.API.Products.GetProductById;

namespace Catalog.API.Products.GetProductByCategory
{
    public record GetProductByCategoryResponse(IEnumerable<Product> Products);
    public class GetProductByCategoryEndpoint : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapGet("/products/category/{category}", async (string category, ISender sender, IMapper mapper) =>
            {
                var result = await sender.Send(new GetProductByCategoryQuery(category));
                var response = mapper.Map<GetProductByCategoryResponse>(result);
                return response;

            }).WithName("GetProductByCategory")
            .Produces<GetProductByCategoryResponse>(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .WithSummary("Get product by category")
            .WithDescription("Get product by category from the catalog");
        }
    }
}
