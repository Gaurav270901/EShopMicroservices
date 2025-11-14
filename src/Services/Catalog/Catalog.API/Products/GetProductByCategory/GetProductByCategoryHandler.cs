
namespace Catalog.API.Products.GetProductByCategory
{
    public record GetProductByCategoryQuery(string Category)  : IQuery<GetProductByCategoryResult>;
    public record GetProductByCategoryResult(IEnumerable<Product> Products);
    public class GetProductByCategoryQueryHandlers(IDocumentSession session, ILogger<GetProductByCategoryQueryHandlers> logger)
        : IQueryHandler<GetProductByCategoryQuery, GetProductByCategoryResult>
    {
        public async Task<GetProductByCategoryResult> Handle(GetProductByCategoryQuery query, CancellationToken cancellationToken)
        {
            var result = await session.Query<Product>()
                .Where(p => p.Category.Contains(query.Category)).ToListAsync();
            if (!result.Any())
            {
                throw new ProductWithCategoryNotFoundException(query.Category);
            }
            return new GetProductByCategoryResult(result);
        }
    }
}
