using Microsoft.Extensions.DependencyInjection;

namespace Invoices.Api.Correlations;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddCorrelation(this IServiceCollection services) => services
        .AddScoped<ICorrelationContextAccessor, CorrelationContextAccessor>();
}