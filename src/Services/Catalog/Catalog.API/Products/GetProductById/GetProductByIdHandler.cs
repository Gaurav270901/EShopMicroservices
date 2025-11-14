
using Catalog.API.Exceptions;

namespace Catalog.API.Products.GetProductById
{
   
    public record GetProductByIdQuery(Guid id) : IQuery<GetProductByIdResult>;
    public record GetProductByIdResult(Product Product);
    internal class GetProductByQueryIdHandler (IDocumentSession session , ILogger<GetProductByQueryIdHandler> logger)
        : IQueryHandler<GetProductByIdQuery, GetProductByIdResult>
    {
        public async Task<GetProductByIdResult> Handle(GetProductByIdQuery request, CancellationToken cancellationToken)
        {
            logger.LogInformation("Handling GetProductByIdQuery for Id: {Id} ", request.id);

            var product = await session.LoadAsync<Product>(request.id, cancellationToken);
            if (product == null)
            {
                logger.LogWarning("Product with Id: {Id} not found.", request.id);
                throw new ProductNotFoundException(request.id);
            }
            return new GetProductByIdResult(product); 

        }
    }
}
