using Basket.API.Data;
using Basket.API.Models;
using BuildingBlocks.CQRS;
using FluentValidation;

namespace Basket.API.Basket.StoreBasket;
public record StoreBasketCommand(ShoppingCart Cart): ICommand<StoreBasketResult>;

public record StoreBasketResult(string Username);

public class StoreBasketCommandValidator : AbstractValidator<StoreBasketCommand>
{
    public StoreBasketCommandValidator()
    {
        RuleFor(x => x.Cart).NotNull().WithMessage("The cart is required");
        RuleFor(x => x.Cart.UserName).NotNull().NotEmpty().WithMessage("The username is required");
    }
}
internal class StoreBasketCommandHandler(IBasketRepository repository): ICommandHandler<StoreBasketCommand, StoreBasketResult>
{
    public async Task<StoreBasketResult> Handle(StoreBasketCommand command, CancellationToken cancellationToken)
    {
        await repository.StoreBasketAsync(command.Cart, cancellationToken);
        return new StoreBasketResult(command.Cart.UserName);
    }
}