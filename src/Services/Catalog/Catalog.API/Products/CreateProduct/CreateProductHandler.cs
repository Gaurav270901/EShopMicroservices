using BuildingBlocks.CQRS;
using MediatR;

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

    internal class CreateProductCommandHandler : ICommandHandler<CreateProductCommand, CreateProductResult>
    {
        //business logic goes here
        public Task<CreateProductResult> Handle(CreateProductCommand command, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
