using BuildingBlocks.CQRS;
using Catalog.API.Models;
using FluentValidation;
using Marten;

namespace Catalog.API.Products.CreateProduct;

public record CreateProductCommand(string Name, List<string> Category, string Description, string ImageFile, decimal Price)
    : ICommand<CreateProductResult>;

public class CreateProductValidator : AbstractValidator<CreateProductCommand>
{
    public CreateProductValidator()
    {
        RuleFor(p => p.Name).NotNull().NotEmpty().WithMessage("Name is required");
        RuleFor(p => p.Category).NotNull().NotEmpty().WithMessage("Category is required");
        RuleFor(p => p.ImageFile).NotNull().NotEmpty().WithMessage("Description is required");
        RuleFor(p => p.Price).NotNull().NotEmpty().WithMessage("Price is required");
        
    }
}

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