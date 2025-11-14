

using JasperFx.Core.Reflection;
using Microsoft.CodeAnalysis.VisualBasic.Syntax;

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

    public class CreateProductCommandValidator : AbstractValidator<CreateProductCommand>
    {
        public CreateProductCommandValidator()
        {
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

    internal class CreateProductCommandHandler(IDocumentSession session ,IValidator<CreateProductCommand> validator ) 
        : ICommandHandler<CreateProductCommand, CreateProductResult>
    {
        //business logic goes here
        public async Task<CreateProductResult> Handle(CreateProductCommand command, CancellationToken cancellationToken)
        {
            var result = await validator.ValidateAsync(command, cancellationToken);
            var errors = result.Errors.Select(x => x.ErrorMessage).ToList();

            if (errors.Any())
            {
                throw new ValidationException(errors.FirstOrDefault());
            }
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

            var createResult = new CreateProductResult(Guid.NewGuid());
            return createResult;
        }
    }
}
