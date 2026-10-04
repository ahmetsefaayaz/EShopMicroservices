using BuildingBlocks.CQRS;
using Ordering.Application.Data;
using Ordering.Application.Dtos;
using Ordering.Domain.Models;
using Ordering.Domain.ValueObjects;

namespace Ordering.Application.Orders.Commands.CreateOrder;





public class CreateOrderCommandHandler(IApplicationDbContext dbContext): ICommandHandler<CreateOrderCommand, CreateOrderResult>
{
    public async Task<CreateOrderResult> Handle(CreateOrderCommand command, CancellationToken cancellationToken)
    {
        var order = CreateNewOrder(command.Order);
        dbContext.Orders.Add(order);
        await dbContext.SaveChangesAsync(cancellationToken);
        return new CreateOrderResult(order.Id.Value);
    }
    private Order CreateNewOrder(OrderDto dto)
    {
        var shippingAddress = Address.Of(dto.ShippingAddress.FirstName, dto.ShippingAddress.LastName,
            dto.ShippingAddress.EmailAddress,
            dto.ShippingAddress.AddressLine, dto.ShippingAddress.Country, dto.ShippingAddress.State,
            dto.ShippingAddress.ZipCode);
        var billingAddress = Address.Of(dto.BillingAddress.FirstName, dto.BillingAddress.LastName,
            dto.BillingAddress.EmailAddress,
            dto.BillingAddress.AddressLine, dto.BillingAddress.Country, dto.BillingAddress.State,
            dto.BillingAddress.ZipCode);
        var newOrder = Order.Create(
            id: OrderId.Of(new Guid()),
            customerId: CustomerId.Of(dto.CustomerId),
            orderName: OrderName.Of(dto.OrderName),
            shippingAddress: shippingAddress,
            billingAddress: billingAddress,
            payment: Payment.Of(dto.Payment.CardName, dto.Payment.CardNumber, dto.Payment.Expiration, dto.Payment.Cvv,
                dto.Payment.PaymentMethod));
        foreach (var item in dto.OrderItems)
        {
            newOrder.Add(ProductId.Of(item.ProductId), item.Quantity, item.Price);
        }
        return newOrder;
    }
}