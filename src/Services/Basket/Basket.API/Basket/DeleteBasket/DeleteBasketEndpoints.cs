using Basket.API.Basket.DeleteBasket.Catalog.API.Products.DeleteProduct;

namespace Basket.API.Basket.DeleteBasket
{
    public record DeleteBasketRequest(Guid Id);

    public record DeleteBasketResponse(bool IsSuccess);

    public class DeleteBasketEndpoint : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapDelete("/basket/{userName}", async (string userName, ISender sender) =>
            {
                var result = await sender.Send(new DeleteBasketCommand(userName));

                var response = result.Adapt<DeleteBasketResponse>();

                return Results.Ok(response);
            })
            .WithName("DeleteBasket")                                       //
            .Produces<DeleteBasketResponse>(StatusCodes.Status200OK)        // Carter extension method
            .ProducesProblem(StatusCodes.Status400BadRequest)               //
            .ProducesProblem(StatusCodes.Status404NotFound)                //
            .WithSummary("Delete Basket")
            .WithDescription("Delete Basket");
        }
    }
}
