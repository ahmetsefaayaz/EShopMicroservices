using System.Text.Json;
using Basket.API.Models;
using Microsoft.Extensions.Caching.Distributed;

namespace Basket.API.Data;

public class CachedBasketRepository(IBasketRepository repository, IDistributedCache cache): IBasketRepository
{
    public async Task<ShoppingCart> GetBasketAsync(string username, CancellationToken cancellationToken)
    {
        var cachedBasket = await cache.GetStringAsync(username, cancellationToken);
        if(!string.IsNullOrEmpty(cachedBasket))
            return JsonSerializer.Deserialize<ShoppingCart>(cachedBasket)!;
        
        var basket = await repository.GetBasketAsync(username, cancellationToken);
        await cache.SetStringAsync(username, JsonSerializer.Serialize(basket), cancellationToken);
        return basket;
    }

    public async Task<ShoppingCart> StoreBasketAsync(ShoppingCart cart, CancellationToken cancellationToken)
    {
        await repository.StoreBasketAsync(cart, cancellationToken);
        await cache.SetStringAsync(cart.UserName,  JsonSerializer.Serialize(cart), cancellationToken);
        return cart;
    }

    public async Task<bool> DeleteBasketAsync(string username, CancellationToken cancellationToken)
    {
        await repository.DeleteBasketAsync(username, cancellationToken);
        await cache.RemoveAsync(username,  cancellationToken);
        return true;
    }
}