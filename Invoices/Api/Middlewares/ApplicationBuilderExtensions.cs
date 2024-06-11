using Microsoft.AspNetCore.Builder;

namespace Invoices.Api.Middlewares;

public static class ApplicationBuilderExtensions
{
    public static IApplicationBuilder UseCorrelationIdMiddleware(
        this IApplicationBuilder builder)
    {
        return builder.UseMiddleware<CorrelationIdMiddleware>();
    }
}