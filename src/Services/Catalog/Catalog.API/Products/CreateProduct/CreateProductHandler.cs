
namespace Catalog.API.Products.CreateProduct
{
//You send a command(CreateProductCommand) to MediatR.
//MediatR checks which handler is responsible for that type (CreateProductHandler).
//It runs the Handle() method in that handler.
//The handler executes logic and returns a result (CreateProductResult).
//MediatR gives that result back to you.

    public record CreateProductCommand(string Name , List<string> Category, string Description, string ImageFile, decimal Price) 
        : ICommand<CreateProductResult>; // when mediatr see request coming as this record then it will trigger handler 
    public record CreateProductResult(Guid Id);

    internal class CreateProductCommandHandler(IDocumentSession session) 
        : ICommandHandler<CreateProductCommand, CreateProductResult>
    {
        //business logic goes here
        public async Task<CreateProductResult> Handle(CreateProductCommand command, CancellationToken cancellationToken)
        {
            var product = new Product
            {
                Name = command.Name,
                Category = command.Category,
                Description = command.Description,
                ImageFile = command.ImageFile,
                Price = command.Price
            };

            //marten will check db and if there is not schema for product it will automtically get created in postgres
            session.Store(product); 
            await session.SaveChangesAsync(cancellationToken); //persist to database

            var result = new CreateProductResult(Guid.NewGuid());
            return result;
        }
    }
}
