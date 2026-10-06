using Basket.API.Data;
using Basket.API.Dtos;
using BuildingBlock.Messaging.Events;
using BuildingBlocks.CQRS;
using FluentValidation;
using Mapster;
using MassTransit;

namespace Basket.API.Basket.CheckoutBasket;


public record CheckoutBasketCommand (BasketCheckoutDto BasketCheckoutDto): ICommand<CheckoutBasketResult>;
public record CheckoutBasketResult(bool IsSuccess);

public class CheckoutBasketCommandValidator : AbstractValidator<CheckoutBasketCommand>
{
    public CheckoutBasketCommandValidator()
    {
        
        RuleFor(b => b.BasketCheckoutDto).NotNull().WithMessage("BasketCheckoutDto is required");
        RuleFor(b => b.BasketCheckoutDto.UserName).NotEmpty().WithMessage("UserName is required");
    }
}
public class CheckoutBasketCommandHandler(IBasketRepository repository, IPublishEndpoint publishEndpoint)
    : ICommandHandler<CheckoutBasketCommand, CheckoutBasketResult>
{
    public async Task<CheckoutBasketResult> Handle(CheckoutBasketCommand command, CancellationToken cancellationToken)
    {
        var basket = await repository.GetBasketAsync(command.BasketCheckoutDto.UserName, cancellationToken);
        if (basket == null) return new CheckoutBasketResult(false);
        var eventMessage = command.BasketCheckoutDto.Adapt<BasketCheckoutEvent>();
        eventMessage.TotalPrice = basket.TotalPrice;
        await publishEndpoint.Publish(eventMessage, cancellationToken);
        await repository.DeleteBasketAsync(basket.UserName, cancellationToken);
        return new CheckoutBasketResult(true);
    }
}