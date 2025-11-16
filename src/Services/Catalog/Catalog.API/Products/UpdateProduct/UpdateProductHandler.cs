
namespace Catalog.API.Products.UpdateProduct
{
    public record UpdateProductCommand(Guid Id , string Name, List<string> Category, string Description, string ImageFile, decimal Price)
        : ICommand<UpdateProductResult>;
    public record UpdateProductResult(bool IsSuccess);

    public class UpdateProductCommandValidator : AbstractValidator<UpdateProductCommand>
    {
        public UpdateProductCommandValidator()
        {
            RuleFor(x=> x.Id)
                .NotEmpty().WithMessage("Product ID is required.");

            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Product name is required.")
                .MaximumLength(100).WithMessage("Product name must not exceed 100 characters.");

            RuleFor(x => x.Category)
                .NotNull().WithMessage("Category is required.")
                .Must(categories => categories != null && categories.Count > 0)
                .WithMessage("At least one category must be specified.");

            RuleFor(x => x.Description)
                .NotEmpty().WithMessage("Product description is required.")
                .MaximumLength(500).WithMessage("Product description must not exceed 500 characters.");

            RuleFor(x => x.ImageFile)
                .NotEmpty().WithMessage("Image file is required.")
                .MaximumLength(200).WithMessage("Image file path must not exceed 200 characters.");

            RuleFor(x => x.Price)
                .GreaterThan(0).WithMessage("Price must be greater than zero.");
        }
    }
    public class UpdateProductCommandHandler(IDocumentSession session) 
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
