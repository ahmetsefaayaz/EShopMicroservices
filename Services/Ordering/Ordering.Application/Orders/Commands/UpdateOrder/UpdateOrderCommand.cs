using BuildingBlocks.CQRS;
using FluentValidation;
using Ordering.Application.Dtos;

namespace Ordering.Application.Orders.Commands.UpdateOrder;


public record UpdateOrderResult(bool IsSuccess);

public class UpdateOrderCommandValidator : AbstractValidator<UpdateOrderCommand>
{
    public UpdateOrderCommandValidator()
    {
        RuleFor(o => o.Order.Id).NotEmpty().WithMessage("Id is required");
        RuleFor(o => o.Order.OrderName).NotEmpty().WithMessage("OrderName is required");
        RuleFor(o => o.Order.CustomerId).NotNull().WithMessage("CustomerId is required");
    }
}

public record UpdateOrderCommand(OrderDto Order): ICommand<UpdateOrderResult>;