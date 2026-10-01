using Basket.API.Exceptions;
using Basket.API.Models;
using Marten;

namespace Basket.API.Data;

public class BasketRepository(IDocumentSession session): IBasketRepository
{
    
    public async Task<ShoppingCart> GetBasketAsync(string username, CancellationToken cancellationToken)
    {
        var basket = await session.LoadAsync<ShoppingCart>(username, cancellationToken);
        return basket ?? throw new BasketNotFoundException(username);
    }

    public async Task<ShoppingCart> StoreBasketAsync(ShoppingCart cart, CancellationToken cancellationToken)
    {
        session.Store(cart);
        await session.SaveChangesAsync(cancellationToken);
        return cart;
    }

    public async Task<bool> DeleteBasketAsync(string username, CancellationToken cancellationToken)
    {
        session.Delete<ShoppingCart>(username);
        await session.SaveChangesAsync(cancellationToken);
        return true;
    }
}
