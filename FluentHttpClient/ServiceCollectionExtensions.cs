using Microsoft.Extensions.DependencyInjection;

namespace FluentHttpClient;
public static class ServiceCollectionExtensions
{
    /// <summary>
    /// Adds the <see cref="FluentClient"/> as a typed HTTP client service implementing <see cref="IFluentClient"/>.
    /// </summary>
    /// <param name="services">The <see cref="IServiceCollection"/> to add the service to.</param>
    /// <returns>The updated <see cref="IServiceCollection"/>.</returns>
    public static IServiceCollection AddFluentHttpClient(this IServiceCollection services)
    {
        services.AddHttpClient<IFluentClient, FluentClient>();
        return services;
    }
}
