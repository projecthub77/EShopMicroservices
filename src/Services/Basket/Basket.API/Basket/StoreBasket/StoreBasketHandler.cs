
namespace Basket.API.Basket.StoreBasket
{
    public record StoreBasketCommand(ShoppingCart Cart)
        : ICommand<StoreBasketResult>;

    public record StoreBasketResult(string UserName);

    public class StoreBasketCommandValidator : AbstractValidator<StoreBasketCommand>
    {
        public StoreBasketCommandValidator()
        {
            RuleFor(x => x.Cart).NotNull().WithMessage("Cart can not be null");
            RuleFor(x => x.Cart.UserName).NotEmpty().WithMessage("Username is required");
        }
    }

    public class StoreBasketCommandHandler
                    : ICommandHandler<StoreBasketCommand, StoreBasketResult>
    {
        public async Task<StoreBasketResult> Handle(StoreBasketCommand command, CancellationToken cancellationToken)
        {
            //var result = await validator.ValidateAsync(command, cancellationToken);       
            //var errors = result.Errors.Select(x => x.ErrorMessage).ToList();                  Questa parta adesso è gestita dal gestore ValidationBehavior
            //if (errors.Any())
            //{
            //    throw new ValidationException(errors.FirstOrDefault());
            //}

            //logger.LogInformation("CreateProductCommandHandler.Handle called with {@Command}", command);   //Anche questa è centralizzata nel LoggingBehavior

            ShoppingCart cart = command.Cart;

            cart = new ShoppingCart
            {
                //Name = command.Name,
                //Category = command.Category,
                //Description = command.Description,
                //ImageFile = command.ImageFile,
                //Price = command.Price
            };

            //session.Store(product);
            //await session.SaveChangesAsync(cancellationToken);

            return new StoreBasketResult("swn");

            //throw new NotImplementedException();
        }
    }
}
