using BuildingBlocks.CQRS;
using Carter;
using Catalog.API.Models;
using Mapster;
using MediatR;

namespace Catalog.API.Products.UpdateProduct;

public record UpdateProductRequest(
    string Name,
    List<string> Category,
    string Description,
    string ImageFile,
    decimal Price);
public record UpdateProductResponse(bool IsSuccess);

public class UpdateProductEndpoint: ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPut("/products/{id:guid}", async (Guid id, ISender sender, UpdateProductRequest request) =>
        {
            var command = request.Adapt<UpdateProductCommand>() with { Id = id };
            var result = await sender.Send(command);
            var response = result.Adapt<UpdateProductResponse>();
            return Results.Ok(response);
        })
        .WithName("UpdateProduct")
        .Produces<UpdateProductResponse>(StatusCodes.Status200OK)
        .ProducesProblem(StatusCodes.Status400BadRequest)
        .WithSummary("UpdateProduct")
        .WithDescription("UpdateProduct");
    }
}