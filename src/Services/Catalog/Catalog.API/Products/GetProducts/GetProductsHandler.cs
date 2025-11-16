using Marten.Pagination;

namespace Catalog.API.Products.GetProduct
{
    public record GetProductsQuery(int? PageNumber = 1, int? PageSize = 10) : IQuery<GetProductResult>;
    public record GetProductResult(List<Product> Products) ;
    public class GetProductsQueryHandler(IDocumentSession session) : IQueryHandler<GetProductsQuery, GetProductResult>
    {
        public async Task<GetProductResult> Handle(GetProductsQuery query, CancellationToken cancellationToken)
        {
            var result =  await session.Query<Product>().ToPagedListAsync(query.PageNumber ?? 1, query.PageSize ?? 10,cancellationToken);
            return new GetProductResult(result.ToList());
        }
    }
}
