using System;
using MassTransit;

namespace Invoices.Worker.Sagas;

public class RaiseInvoiceSagaData : SagaStateMachineInstance
{
    public Guid CorrelationId { get; set; }
    public string CurrentState { get; set; }
    public Guid? DebtorId { get; set; }
    public bool DebtorCreated { get; set; }
    public bool InvoiceRaised { get; set; }
}