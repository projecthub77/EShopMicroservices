using Catalog.API.Products.CreateProduct;
using Catalog.API.Products.GetProductByCategory;

namespace Catalog.API.Products.UpdateProduct
{
    public record UpdateProductCommand(Guid Id, string Name, List<string> Category, string Description, string ImageFile, decimal Price)
         : ICommand<UpdateProductResult>;

    public record UpdateProductResult(bool IsSuccess);


    public class UpdateProductCommandHandler(IDocumentSession session, ILogger<UpdateProductCommandHandler> logger) : ICommandHandler<UpdateProductCommand, UpdateProductResult>
    {
        public async Task<UpdateProductResult> Handle(UpdateProductCommand commad, CancellationToken cancellationToken)
        {
            logger.LogInformation("UpdateProductCommandHandler.Handle called with {@Command}", commad);

            var product = await session.LoadAsync<Product>(commad.Id, cancellationToken);

            if (product is null) 
            {
                throw new ProductNotFoundException();   
            }

            product.Name = commad.Name;
            product.Category = commad.Category;
            product.Description = commad.Description;
            product.ImageFile = commad.ImageFile;
            product.Price = commad.Price;
                
            session.Update(product);
            await session.SaveChangesAsync(cancellationToken);

            return new UpdateProductResult(true);
        }
    }
}
