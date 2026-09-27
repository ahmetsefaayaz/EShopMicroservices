using BuildingBlocks.CQRS;
using Catalog.API.Models;
using Marten;
using MediatR;

namespace Catalog.API.Products.CreateProduct;

public record CreateProductCommand(string Name, List<string> Category, string Description, string ImageFile, decimal Price)
    : ICommand<CreateProductResult>;

public record CreateProductResult(Guid Id);
internal class CreateProductCommandHandler(IDocumentSession session): 
    ICommandHandler<CreateProductCommand, CreateProductResult>
{
    public async Task<CreateProductResult> Handle(CreateProductCommand command, CancellationToken cancellationToken)
    {
        var product = new Product
        {
            Category = command.Category,
            Description = command.Description,
            ImageFile = command.ImageFile,
            Name = command.Name,
            Price = command.Price
        };
        session.Store(product);
        await session.SaveChangesAsync(cancellationToken);
        return new CreateProductResult(product.Id);
    }
}