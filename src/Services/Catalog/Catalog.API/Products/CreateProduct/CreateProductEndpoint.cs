
namespace Catalog.API.Products.CreateProduct
{
    //nella Cartella Products con la Vertical Slice Architecture raggruppiamo tutti i casi d'uso di un oggetto.
    public record CreateProductRequest(string Name, List<string> Category, string Description, string ImageFile, decimal Price);

    public record CreateProductResponse(Guid Id);
    
    public class CreateProductEndpoint : ICarterModule
    {        
        public void AddRoutes(IEndpointRouteBuilder app)
        {            
            app.MapPost("/products", async (CreateProductRequest request, ISender sender) =>
            {
                var command = request.Adapt<CreateProductCommand>();  // mappo la richiesta sull'oggetto CreateProductCommand
                                    
                var result = await sender.Send(command);            // la inviamo tramite il mediatr che attiverà la classe handler corrispondente (mediatr gestisce la logica di business tramite i comandi)

                var response = result.Adapt<CreateProductResponse>();  // alla fine si riadatta la risposta alla classe CreateProductResponse                

                return Results.Created($"/products/{response.Id}", response);  // e si torna al client
            })
                .WithName("CreateProduct")                                      //
                .Produces<CreateProductResponse>(StatusCodes.Status201Created)  // Carter extension method
                .ProducesProblem(StatusCodes.Status400BadRequest)               //
                .WithSummary("Create Product")
                .WithDescription("Create Product");
        }
    }
}
