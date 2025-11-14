
using System.Net;

namespace Catalog.API.Products.UpdateProduct
{
    public record UpdateProductRequest(Guid Id, string Name, List<string> Category, string Description, string ImageFile, decimal Price);
    public record UpdateProductResponse(bool IsSuccess);
    public class UpdateProductEndpoint : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapPut("/products",
                async (UpdateProductRequest request, ISender sender, IMapper mapper) =>
            {
                var product = mapper.Map<UpdateProductCommand>(request);
                var updateProduct = await sender.Send(product);
                var response = mapper.Map<UpdateProductResponse>(updateProduct);
                return Results.Ok(response);

            }).WithName("UpdateProduct")
            .WithDescription("update product")
            .Produces<UpdateProductResponse>(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .WithSummary("update product");
        }
    }
}
