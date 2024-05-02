using Catalog.API.Products.GetProducts;

namespace Catalog.API.Products.GetProductByCategory
{
    public record GetProductByCategoryResponse(IEnumerable<Product> Products);

    public class GetProductByCategoryEndpoint : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapGet("/products/category/{category}", async (string category , ISender sender) =>
            {
                var result = await sender.Send(new GetProductByCategoryQuery(category));

                var response = result.Adapt<GetProductByCategoryResponse>(); // se i nomi dei parametri di questo oggetto (Products) hanno lo stesso nome
                                                                   // o firma, come ad esempio sopra e nel productHandler, Mapster si può usare così
                                                                   // a volo senza ulteriori configuarazioni.
                return Results.Ok(response);
            })
            .WithName("GetProductByCategory")                                    //
            .Produces<GetProductByCategoryResponse>(StatusCodes.Status200OK)        // Carter extension method
            .ProducesProblem(StatusCodes.Status400BadRequest)                       //
            .WithSummary("Get Products by category")
            .WithDescription("Get Products by category");
        }
    }
}
