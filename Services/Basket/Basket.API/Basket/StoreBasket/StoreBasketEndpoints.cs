using Basket.API.Models;
using Carter;
using Mapster;
using MediatR;

namespace Basket.API.Basket.StoreBasket;

public record StoreBasketRequest(ShoppingCart Cart);
public record StoreBasketResponse(string Username);

public class StoreBasketEndpoints: ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPost("/basket", async (ISender sender, StoreBasketRequest request) =>
        {
            var command = request.Adapt<StoreBasketCommand>();
            var result = await sender.Send(command);
            var response = result.Adapt<StoreBasketResponse>();
            return Results.Created($"/basket/{response.Username}", response);
            
        })
        .WithName("CreateBasket")
        .Produces<StoreBasketResponse>(StatusCodes.Status201Created)
        .ProducesProblem(StatusCodes.Status400BadRequest)
        .WithSummary("Create Basket By Username")
        .WithDescription("Create Basket By Username");
    }
}