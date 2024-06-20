namespace Invoices.Api.Correlations;

public interface ICorrelationContextAccessor
{
    CorrelationContext CorrelationContext { get; set; }
}