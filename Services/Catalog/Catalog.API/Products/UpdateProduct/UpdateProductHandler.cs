using BuildingBlocks.CQRS;
using Catalog.API.Exceptions;
using Catalog.API.Models;
using FluentValidation;
using Marten;

namespace Catalog.API.Products.UpdateProduct;

public record UpdateProductCommand(Guid Id, string Name, List<string> Category, string Description, string ImageFile, decimal Price):
    ICommand<UpdateProductResult>;
public record UpdateProductResult(bool IsSuccess);

public class UpdateProductValidator : AbstractValidator<UpdateProductCommand>
{
    public UpdateProductValidator()
    {
        RuleFor(x => x.Id).NotNull().NotEmpty().WithMessage("Id is required");
        RuleFor(x => x.Name).NotNull().NotEmpty().WithMessage("Name is required")
            .Length(2, 150).WithMessage("Name must be between 2 and 150 characters");
        RuleFor(x => x.Price).NotNull().NotEmpty().WithMessage("Price is required");
        
    }
}

internal class UpdateProductCommandHandler (IDocumentSession session)
    : ICommandHandler<UpdateProductCommand, UpdateProductResult>
{
    public async Task<UpdateProductResult> Handle(UpdateProductCommand command, CancellationToken cancellationToken)
    {
        var product = await session.LoadAsync<Product>(command.Id, cancellationToken);
        if (product is null)
        {
            throw new ProductNotFoundException(command.Id);
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