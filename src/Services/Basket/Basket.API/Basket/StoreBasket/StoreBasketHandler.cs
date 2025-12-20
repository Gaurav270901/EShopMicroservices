

using Discount.Grpc;
using JasperFx.Events.Daemon;

namespace Basket.API.Basket.StoreBasket
{
    public record StoreBasketCommand(ShoppingCart Cart) : ICommand<StoreBasketResult>;
    public record StoreBasketResult(string UserName);

    public class StoreBasketCommandValidator : AbstractValidator<StoreBasketCommand>
    {
        public StoreBasketCommandValidator()
        {
            RuleFor(x => x.Cart).NotNull().WithMessage("Cart can not be null");
            RuleFor(x => x.Cart.UserName).NotEmpty().WithMessage("UserName is required");
        }
    }
    //for using grpc service we need to inject discount service client in handler
    public class StoreBasketCommandHandler
        (IBasketRepository repository,DiscountProtoService.DiscountProtoServiceClient dicountProto) :
        ICommandHandler<StoreBasketCommand, StoreBasketResult>
    {
        public async Task<StoreBasketResult> Handle(StoreBasketCommand command, CancellationToken cancellationToken)
        {
            await DeductDiscount(command.Cart, cancellationToken);

            await repository.StoreBasket(command.Cart , cancellationToken);

            return new StoreBasketResult(command.Cart.UserName);
        }

        private async Task DeductDiscount(ShoppingCart cart, CancellationToken cancellationToken)
        {
            //communicate with discount.grpc and calculate latest prices of product
            foreach (var item in cart.Items)
            {
                //create a discount request
                var discountRequest = new GetDiscountRequest { ProductName = item.ProductName };
                //call grpc service to get discount
                var discountResponse = await dicountProto.GetDiscountAsync(discountRequest, cancellationToken: cancellationToken);
                //apply discount to item price
                item.Price -= discountResponse.Amount;
            }
        }
    }
}
