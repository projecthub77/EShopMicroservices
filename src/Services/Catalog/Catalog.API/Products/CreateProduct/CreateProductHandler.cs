
//nella Cartella Products con la Vertical Slice Architecture raggruppiamo tutti i casi d'uso di un oggetto.
using FluentValidation;

namespace Catalog.API.Products.CreateProduct
{
    public record CreateProductCommand(string Name, List<string> Category, string Description, string ImageFile, decimal Price)
        : ICommand<CreateProductResult>;

    public record CreateProductResult(Guid Id);

    public class CreateProductCommandValidator : AbstractValidator<CreateProductCommand>
    {
        public CreateProductCommandValidator()
        {
            RuleFor(x => x.Name).NotEmpty().WithMessage("Name is Required");
            RuleFor(x => x.Category).NotEmpty().WithMessage("Category is Required");
            RuleFor(x => x.Description).NotEmpty().WithMessage("Description is Required");
            RuleFor(x => x.ImageFile).NotEmpty().WithMessage("ImageFile is Required");
            RuleFor(x => x.Price).GreaterThan(0).WithMessage("Price must be greater than 0");
        }
    }

    //l'Handler gestisce la funzionalità principale delegando la responsabilità di validazione alla pipelinde del mediator.

    public class CreateProductCommandHandler (IDocumentSession session, ILogger<CreateProductCommandHandler> logger/*, IValidator<CreateProductCommand> validator*/) 
                    : ICommandHandler<CreateProductCommand, CreateProductResult>
    {
        public async Task<CreateProductResult> Handle(CreateProductCommand command, CancellationToken cancellationToken)
        {
            //var result = await validator.ValidateAsync(command, cancellationToken);       
            //var errors = result.Errors.Select(x => x.ErrorMessage).ToList();                  Questa parta adesso è gestita dal gestore ValidationBehavior
            //if (errors.Any())
            //{
            //    throw new ValidationException(errors.FirstOrDefault());
            //}

            logger.LogInformation("CreateProductCommandHandler.Handle called with {@Command}", command);

            var product = new Product
            {
                Name = command.Name,
                Category = command.Category,
                Description = command.Description,
                ImageFile = command.ImageFile,
                Price = command.Price
            };

            session.Store(product);
            await session.SaveChangesAsync(cancellationToken);

            return new CreateProductResult(product.Id);

            //throw new NotImplementedException();
        }
    }
}
