using System;

namespace Invoices.Api.Correlations;

public class CorrelationNotCreatedException : Exception
{
    public CorrelationNotCreatedException()
    {
    }

    public CorrelationNotCreatedException(string message) : base(message)
    {
    }

    public CorrelationNotCreatedException(string message, Exception inner) : base(message, inner)
    {
    }
}