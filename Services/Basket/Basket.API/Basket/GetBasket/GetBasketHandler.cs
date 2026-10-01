using Basket.API.Data;
using Basket.API.Models;
using BuildingBlocks.CQRS;

namespace Basket.API.Basket.GetBasket;

public record GetBasketQuery(string Username): IQuery<GetBasketResult>;
public record GetBasketResult(ShoppingCart Cart);

internal class GetBasketQueryHandler(IBasketRepository repository): IQueryHandler<GetBasketQuery, GetBasketResult>
{
    public async Task<GetBasketResult> Handle(GetBasketQuery query, CancellationToken cancellationToken)
    {
        var cart = await repository.GetBasketAsync(query.Username, cancellationToken);
        return new GetBasketResult(cart);
    }
}