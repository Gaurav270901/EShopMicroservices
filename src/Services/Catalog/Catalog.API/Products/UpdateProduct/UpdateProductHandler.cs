
namespace Catalog.API.Products.UpdateProduct
{
    public record UpdateProductCommand(Guid Id , string Name, List<string> Category, string Description, string ImageFile, decimal Price)
        : ICommand<UpdateProductResult>;
    public record UpdateProductResult(bool IsSuccess);
    public class UpdateProductCommandHandler(IDocumentSession session , ILogger<UpdateProductCommandHandler> logger) 
        : ICommandHandler<UpdateProductCommand, UpdateProductResult>
    {
        public async Task<UpdateProductResult> Handle(UpdateProductCommand command, CancellationToken cancellationToken)
        {
            var existingProduct = await session.LoadAsync<Product>(command.Id, cancellationToken);
            if (existingProduct == null)
            {
                throw new ProductNotFoundException(command.Id);
            }
            existingProduct.Name = command.Name;
            existingProduct.ImageFile = command.ImageFile;
            existingProduct.Description = command.Description;
            existingProduct.Price = command.Price;
            existingProduct.Category = command.Category;

            session.Store(existingProduct);
            await session.SaveChangesAsync(cancellationToken);
            return new UpdateProductResult(true);
        }
    }
}
