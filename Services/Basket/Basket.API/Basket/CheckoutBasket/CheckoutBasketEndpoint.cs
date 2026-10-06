using Basket.API.Dtos;
using Carter;
using Mapster;
using MediatR;

namespace Basket.API.Basket.CheckoutBasket;

public record CheckoutBasketRequest(BasketCheckoutDto BasketCheckoutDto);
public record CheckoutBasketResponse(bool IsSuccess);
public class CheckoutBasketEndpoint: ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPost("/basket/checkout", async (ISender sender, CheckoutBasketRequest request) =>
            {
                var command = request.Adapt<CheckoutBasketCommand>();
                var result = await sender.Send(command);
                var response = result.Adapt<CheckoutBasketResponse>();
                return Results.Ok(response);
            })
            .WithDescription("Checkout basket")
            .WithName("CheckoutBasket")
            .WithDescription("Checkout basket")
            .Produces<CheckoutBasketResponse>(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status400BadRequest);
    }
}