
using Catalog.API.Products.CreateProduct;

namespace Catalog.API.Products.GetProducts
{
    public record GetProductResponse(IEnumerable<Product> Products);

    public class GetProductsEndpoint : ICarterModule   // Carter usato per creare endpoint minimal api
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapGet("/products", async (ISender sender) =>
            {
                var result = await sender.Send(new GetProductsQuery());

                var response = result.Adapt<GetProductResponse>(); // se i nomi dei parametri di questo oggetto (Products) hanno lo stesso nome
                                                                      // o firma, come ad esempio sopra e nel productHandler, Mapster si può usare così
                                                                      // a volo senza ulteriori configuarazioni.
                return Results.Ok(response);
            })
            .WithName("GetProducts")
            .Produces<GetProductResponse>(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .WithSummary("Get Products")
            .WithDescription("Get Products");
        }
    }
}
