using Catalog.API.Models;
using Marten;
using Marten.Schema;

namespace Catalog.API.Data;

public class CatalogInitialData: IInitialData
{
    public async Task Populate(IDocumentStore store, CancellationToken cancellation)
    {
        await using var session = store.LightweightSession();
        if (await session.Query<Product>().AnyAsync(cancellation)) return;
        
        session.Store<Product>(GetPreconfiguredProducts());
        await session.SaveChangesAsync(cancellation);
        
    }

    private static IEnumerable<Product> GetPreconfiguredProducts() => new List<Product>
    {
        new Product
        {
            Id = Guid.NewGuid(),
            Name = "IphoneX",
            Description = "Iphone is good",
            ImageFile = "product-1.png",
            Price = 950.00M,
            Category = new List<string> { "smart phone" }
        },
        new Product
        {
            Id = Guid.NewGuid(),
            Name = "Samsung 10",
            Description = "Samsung 10 is good",
            ImageFile = "product-2.png",
            Price = 870.00M,
        },
        new Product
        {
            Id = Guid.NewGuid(),
            Name = "Samsung 11",
            Description = "Samsung 11 is good",
            ImageFile = "product-3.png",
            Price = 1060.00M
        },
        new Product
        {
            Id = Guid.NewGuid(),
            Name = "Samsung 12",
            Description = "Samsung 12 is good",
            ImageFile = "product-4.png",
            Price = 1200.00M
        }
    };

}