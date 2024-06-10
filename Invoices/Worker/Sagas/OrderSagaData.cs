using System;
using MassTransit;

namespace Invoices.Worker.Sagas;

public class OrderSagaData : SagaStateMachineInstance
{
    public Guid CorrelationId { get; set; }
    public string CurrentState { get; set; }
    public Guid? DebtorId { get; set; }
    public bool DebtorCreated { get; set; }
    public bool InvoiceCreated { get; set; }
    public string Name { get; set; }
    public decimal Amount { get; set; }
    public string Currency { get; set; }
    public string DebtorEmail { get; set; }
    public string Url { get; set; }
}