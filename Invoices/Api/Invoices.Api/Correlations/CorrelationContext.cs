using System;

namespace Invoices.Api.Correlations;

public class CorrelationContext
{
    public Guid CorrelationId { get; private init; }

    public static CorrelationContext CreateFromGuid(Guid correlationId) => new()
    {
        CorrelationId = correlationId
    };

    public static CorrelationContext Create() => new()
    {
        CorrelationId = Guid.NewGuid()
    };
}