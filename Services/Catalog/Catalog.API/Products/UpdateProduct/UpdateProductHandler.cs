using BuildingBlocks.CQRS;
using Catalog.API.Exceptions;
using Catalog.API.Models;
using Marten;

namespace Catalog.API.Products.UpdateProduct;

public record UpdateProductCommand(Guid Id, string Name, List<string> Category, string Description, string ImageFile, decimal Price):
    ICommand<UpdateProductResult>;
public record UpdateProductResult(bool IsSuccess);
internal class UpdateProductHandler (IDocumentSession session)
    : ICommandHandler<UpdateProductCommand, UpdateProductResult>
{
    public async Task<UpdateProductResult> Handle(UpdateProductCommand command, CancellationToken cancellationToken)
    {
        var product = await session.LoadAsync<Product>(command.Id, cancellationToken);
        if (product is null)
        {
            throw new ProductNotFoundException();
        }
        product = UpdateProduct(product, command.Name, command.Category, command.Description, command.ImageFile, command.Price);
        session.Update(product);
        await session.SaveChangesAsync(cancellationToken);
        return new UpdateProductResult(true);
    }

    public Product UpdateProduct(Product product, string Name, List<string> Category, string Description,
        string ImageFile, decimal Price)
    {
        product.Category = Category;
        product.Name =  Name;
        product.Description = Description;
        product.ImageFile = ImageFile;
        product.Price = Price;
        return product;
    }
}