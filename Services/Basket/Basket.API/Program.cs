using Basket.API.Data;
using BuildingBlocks.Behaviors;
using BuildingBlocks.Exceptions.Handler;
using Carter;
using Marten;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCarter();
builder.Services.AddMediatR(config =>
{
    config.RegisterServicesFromAssembly(typeof(Program).Assembly);
    config.AddOpenBehavior(typeof(LoggingBehavior<,>));
});
builder.Services.AddMarten(opts =>
{
    opts.Connection(builder.Configuration.GetConnectionString("DefaultConnection")!);
}).UseLightweightSessions();
builder.Services.AddScoped<IBasketRepository, BasketRepository>();
builder.Services.AddExceptionHandler<CustomExceptionHandler>();

var app = builder.Build();

app.MapCarter();

app.UseExceptionHandler(options => { });

app.Run();