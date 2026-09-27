using Carter;
using Mapster;
using MediatR;

namespace Catalog.API.Products.DeleteProduct;
public record DeleteProductResponse(bool IsSuccess);
public class DeleteProductEndpoint: ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapDelete("/products/{id:guid}", async (ISender sender, Guid id) =>
        {
            var result = await sender.Send(new DeleteProductCommand(id));
            var response = result.Adapt<DeleteProductResponse>();
            return Results.Ok(response);
        })
        .WithName("DeleteProduct")
        .Produces<DeleteProductResponse>(StatusCodes.Status200OK)
        .ProducesProblem(StatusCodes.Status400BadRequest)
        .WithName("DeleteProduct")
        .WithDescription("DeleteProduct");
    }
}