using BuildingBlocks.CQRS;
using Microsoft.EntityFrameworkCore;
using Ordering.Application.Data;
using Ordering.Application.Exceptions;
using Ordering.Domain.ValueObjects;

namespace Ordering.Application.Orders.Commands.DeleteOrder;

public class DeleteOrderCommandHandler(IApplicationDbContext dbContext): ICommandHandler<DeleteOrderCommand, DeleteOrderResult>
{
    public async Task<DeleteOrderResult> Handle(DeleteOrderCommand command, CancellationToken cancellationToken)
    {
        var orderId = OrderId.Of(command.OrderId);
        var order = await dbContext.Orders.FirstOrDefaultAsync(o => o.Id == orderId, cancellationToken);
        if (order is null) throw new OrderNotFoundException(command.OrderId);
        dbContext.Orders.Remove(order);
        await dbContext.SaveChangesAsync(cancellationToken);
        return new DeleteOrderResult(true);
        
    }
}