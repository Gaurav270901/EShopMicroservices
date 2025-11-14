namespace Catalog.API.Products.GetProduct
{
    public record GetProductsQuery() : IQuery<GetProductResult>;
    public record GetProductResult(List<Product> Products) ;
    public class GetProductsQueryHandler(IDocumentSession session , ILogger<GetProductResult> logger) : IQueryHandler<GetProductsQuery, GetProductResult>
    {
        public async Task<GetProductResult> Handle(GetProductsQuery request, CancellationToken cancellationToken)
        {
            var result =  await session.Query<Product>().ToListAsync(cancellationToken);
            logger.LogInformation("getproductsquery {@Result}", result);
            return new GetProductResult(result.ToList());
        }
    }
}
