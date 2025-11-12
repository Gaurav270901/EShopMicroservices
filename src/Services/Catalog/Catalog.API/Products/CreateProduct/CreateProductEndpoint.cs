namespace Catalog.API.Products.CreateProduct
{

    public record CreateProductRequest(string Name, List<string> Category, string Description, string ImageFile, decimal Price);
    public record CreateProductResponse(Guid Id);
    //carter use to route minimal api
    public class CreateProductEndpoint : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapPost("/Products", async (CreateProductRequest request, ISender sender, IMapper mapper) =>
            {
                var command = mapper.Map<CreateProductCommand>(request);
                var result = await sender.Send(command);
                var response = mapper.Map<CreateProductResponse>(result);
                return Results.Created($"/Products/{result.Id}", response);
            })
            .WithName("CreateProduct")
            .Produces<CreateProductResponse>(StatusCodes.Status201Created)
            .WithSummary("Create Product")
            .WithDescription("Create Product");
        }
    }
}
