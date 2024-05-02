using Catalog.API.Products.CreateProduct;

namespace Catalog.API.Products.UpdateProduct
{
    public record UpdateProductRequest(string Name, List<string> Category, string Description, string ImageFile, decimal Price);

    public record UpdateProductResponse(bool IsSuccess);

    public class UpdateProductEndpoint : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapPut("/products", async (UpdateProductCommand request, ISender sender) =>
            {
                var command = request.Adapt<UpdateProductCommand>();  // mappo la richiesta sull'oggetto CreateProductCommand
                
                var result = await sender.Send(command);            // la inviamo tramite il mediatr che attiverà la classe handler corrispondente (mediatr gestisce la logica di business tramite i comandi)

                var response = result.Adapt<UpdateProductResponse>();  // alla fine si riadatta la risposta alla classe CreateProductResponse                

                return Results.Ok(response);  // e si torna al client
            })
                .WithName("UpdateProduct")                                      //
                .Produces<UpdateProductResponse>(StatusCodes.Status201Created)  // Carter extension method
                .ProducesProblem(StatusCodes.Status400BadRequest)               //
                .WithSummary("Update Product")
                .WithDescription("Update Product");
        }
    }
}
