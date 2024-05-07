namespace Basket.API.Basket.DeleteBasket
{     
    namespace Catalog.API.Products.DeleteProduct
    {
        public record DeleteBasketCommand(string UserName)
            : ICommand<DeleteBasketResult>;

        public record DeleteBasketResult(bool IsSuccess);

        public class DeleteBasketCommandValidator : AbstractValidator<DeleteBasketCommand>
        {
            public DeleteBasketCommandValidator()
            {
                RuleFor(x => x.UserName).NotEmpty().WithMessage("UserName is Required");
            }
        }

        internal class DeleteBasketCommandHandler(IBasketRepository repository)
            : ICommandHandler<DeleteBasketCommand, DeleteBasketResult>
        {
            public async Task<DeleteBasketResult> Handle(DeleteBasketCommand command, CancellationToken cancellationToken)
            {
                await repository.DeleteBasket(command.UserName, cancellationToken);                

                return new DeleteBasketResult(true);
            }
        }
    }

}
