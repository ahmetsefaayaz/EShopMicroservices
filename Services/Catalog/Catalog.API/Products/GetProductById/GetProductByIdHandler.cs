using BuildingBlocks.CQRS;
using Catalog.API.Exceptions;
using Catalog.API.Models;
using Mapster;
using Marten;

namespace Catalog.API.Products.GetProductById;

public record GetProductByIdQuery(Guid Id): IQuery<GetProductByIdResponse>;
public record GetProductByIdResponse(Product  Product);

internal class GetProductByIdQueryHandler(IDocumentSession session, ILogger<GetProductByIdQueryHandler> logger)
    : IQueryHandler<GetProductByIdQuery, GetProductByIdResponse>
{
    public async Task<GetProductByIdResponse> Handle(GetProductByIdQuery query, CancellationToken cancellationToken)
    {
        logger.LogInformation("GetProductByIdQueryHandler.Handle");
        var result = await session.Query<Product>().FirstOrDefaultAsync(p  => p.Id == query.Id,  cancellationToken);
        if (result == null) throw new ProductNotFoundException();
        return new GetProductByIdResponse(result);
    }
}