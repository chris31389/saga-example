namespace Invoices.Api.Correlations;

public class CorrelationContextAccessor : ICorrelationContextAccessor
{
    private CorrelationContext? _correlationContext;

    public CorrelationContext CorrelationContext
    {
        get => _correlationContext ?? throw new CorrelationNotCreatedException();
        set => _correlationContext = value;
    }
}