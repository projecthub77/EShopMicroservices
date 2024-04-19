//using Carter;
//using Mapster;
//using MediatR;





using MediatR;

namespace Catalog.API.Products.CreateProduct
{    
    public record CreateProductRequest(string Name, List<string> Category, string Description, string ImageFile, decimal Price);

    public record CreateProductResponse(Guid Id);

    public class CreateProductEndpoint : ICarterModule
    {        
        public void AddRoutes(IEndpointRouteBuilder app)
        {            
            app.MapPost("/products", async (CreateProductRequest request, ISender sender) =>
            {
                var command = request.Adapt<CreateProductCommand>();  // mappo la richiesta sull'oggetto CreateProductCommand
                try
                {                    
                    var result = await sender.Send(command);            // la inviamo tramite il mediatr che attiverà la classe handler corrispondente (mediatr gestisce la logica di business tramite i comandi)

                    var response = result.Adapt<CreateProductResponse>();  // alla fine si riadatta la risposta alla classe CreateProductResponse
                }
                catch (Exception ex)
                {
                    throw new Exception("error:" + ex);
                }

                //return Results.Created($"/products/{response.Id}", response);  // e si torna al client
            })
                .WithName("CreateProduct")
                .Produces<CreateProductResponse>(StatusCodes.Status201Created)
                .ProducesProblem(StatusCodes.Status400BadRequest)
                .WithSummary("Create Product")
                .WithDescription("Create Product");
        }
    }
}
