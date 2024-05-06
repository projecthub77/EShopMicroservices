
namespace Basket.API.Basket.StoreBasket
{
    public record StoreBasketRequest(ShoppingCart Cart);        
    public record StoreBasketResponse(string userName);

    public class StoreBasketEndpoints : ICarterModule
    {        
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapPost("/basket", async (StoreBasketRequest request, ISender sender) =>
            {
                var command = request.Adapt<StoreBasketCommand>();  // mappo la richiesta sull'oggetto CreateProductCommand
                   
                var result = await sender.Send(command);            // la inviamo tramite il mediatr che attiverà la classe handler corrispondente (mediatr gestisce la logica di business tramite i comandi)

                var response = result.Adapt<StoreBasketResponse>();  // alla fine si riadatta la risposta alla classe CreateProductResponse                    

                return Results.Created($"/basket/{response.userName}", response);  // e si torna al client
            })
                .WithName("StoreBasket")                                      //
                .Produces<StoreBasketResponse>(StatusCodes.Status201Created)  // Carter extension method
                .ProducesProblem(StatusCodes.Status400BadRequest)             //
                .WithSummary("Store Basket")
                .WithDescription("Store Basket");
        }        
    }
}
