namespace Ordering.API;

public static class DependencyInjection
{
    //ADDING SERVICES
    public static IServiceCollection AddApiServices(this IServiceCollection services)
    {
        
        return services;
    }
    
    //PIPELINE
    public static WebApplication UseApiServices(this WebApplication app)
    {
        
        return app;
    }
}