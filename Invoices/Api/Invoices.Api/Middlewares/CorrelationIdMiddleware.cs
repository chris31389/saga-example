using System;
using System.Linq;
using System.Threading.Tasks;
using Invoices.Api.Correlations;
using Microsoft.AspNetCore.Http;

namespace Invoices.Api.Middlewares;

public class CorrelationIdMiddleware(RequestDelegate next)
{
    public const string CorrelationKey = "X-Correlation-Id";
    
    public async Task InvokeAsync(HttpContext context, ICorrelationContextAccessor correlationContextAccessor)
    {
        var hasKey = context.Request.Headers.ContainsKey(CorrelationKey);
        CorrelationContext correlationContext;
        if (hasKey)
        {
            var correlationIdString = context.Request.Headers[CorrelationKey].FirstOrDefault();
            var isGuid = Guid.TryParse(correlationIdString, out var correlationId);
            correlationContext = isGuid 
                ? CorrelationContext.CreateFromGuid(correlationId) 
                : CorrelationContext.Create();
        }
        else
        {
            correlationContext = CorrelationContext.Create();
        }

        correlationContextAccessor.CorrelationContext = correlationContext;
        await next(context);
    }
}