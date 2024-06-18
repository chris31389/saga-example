using System;
using Debtors.Messages.CreateOrUpdateDebtorCommand.V1;
using Debtors.Messages.CreateOrUpdateDebtorCompleted.V1;
using Emails.Messages.EmailSent.V1;
using Emails.Messages.SendEmailCommand.V1;
using Invoices.Messages.OrderCreated.V1;
using Invoices.Messages.RaiseInvoiceCommand.V1;
using Invoices.Messages.RaiseInvoiceCompleted.V1;
using MassTransit;
using Microsoft.Extensions.Logging;

namespace Invoices.Worker.Sagas;

public class OrderSaga : MassTransitStateMachine<OrderSagaData>
{
    private readonly ILogger<OrderSaga> _logger;
    public State CreatingOrUpdatingDebtor { get; set; }
    public State RaisingInvoice { get; set; }
    public State SendingInvoice { get; set; }
    public State AwaitingPayment { get; set; }
    public Event<OrderCreatedV1> OrderCreated { get; set; }
    public Event<CreateOrUpdateDebtorCompletedV1> CreateOrUpdateDebtorCompleted { get; set; }
    public Event<RaiseInvoiceCompletedV1> RaiseInvoiceCompleted { get; set; }
    public Event<EmailSentV1> EmailSent { get; set; }

    private void OnThen<T1,T2>(BehaviorContext<T1,T2> context, Guid correlationId) 
        where T1 : class, SagaStateMachineInstance
        where T2 : class =>
        _logger.LogInformation("Consuming {EventName}: {CorrelationId}", context.Event.Name, correlationId);

    private void OnPublish(string typeName, Guid correlationId) =>
        _logger.LogInformation("Publishing {EventName}: {CorrelationId}", typeName, correlationId);

    public OrderSaga(ILogger<OrderSaga> logger)
    {
        _logger = logger;

        InstanceState(x => x.CurrentState);

        // Consider changing the correlation id to a transaction Id so that we can have multiple invoices for a debtor
        Event(() => OrderCreated, e => e.CorrelateById(m => m.Message.CorrelationId));
        Event(() => CreateOrUpdateDebtorCompleted, e => e.CorrelateById(m => m.Message.CorrelationId));
        Event(() => RaiseInvoiceCompleted, e => e.CorrelateById(m => m.Message.CorrelationId));
        Event(() => EmailSent, e => e.CorrelateById(m => m.Message.CorrelationId));

        Initially(
            When(OrderCreated)
                .Then(context =>
                {
                    OnThen(context, context.Message.CorrelationId);
                    context.Saga.OrderId = context.Message.OrderId;
                    context.Saga.CorrelationId = context.Message.CorrelationId;
                    context.Saga.Name = context.Message.Name;
                    context.Saga.Amount = context.Message.Amount;
                    context.Saga.Currency = context.Message.Currency;
                    context.Saga.Email = context.Message.Email;
                })
                .TransitionTo(CreatingOrUpdatingDebtor)
                .Publish(context =>
                {
                    OnPublish(nameof(CreateOrUpdateDebtorCommandV1), context.Message.CorrelationId);
                    return new CreateOrUpdateDebtorCommandV1
                    {
                        CustomerId = context.Message.CustomerId,
                        Name = context.Message.Name,
                        CorrelationId = context.Message.CorrelationId,
                        Email = context.Message.Email,
                    };
                }));

        During(CreatingOrUpdatingDebtor,
            When(CreateOrUpdateDebtorCompleted)
                .Then(context =>
                {
                    OnThen(context, context.Message.CorrelationId);
                    context.Saga.DebtorCreated = true;
                    context.Saga.DebtorId = context.Message.DebtorId;
                })
                .TransitionTo(RaisingInvoice)
                .Publish(context =>
                {
                    OnPublish(nameof(RaiseInvoiceCommandV1), context.Message.CorrelationId);
                    return new RaiseInvoiceCommandV1
                    {
                        CorrelationId = context.Message.CorrelationId,
                        DebtorId = context.Saga.DebtorId,
                        Currency = context.Saga.Currency,
                        Value = context.Saga.Amount
                    };
                }));

        During(RaisingInvoice,
            When(RaiseInvoiceCompleted)
                .Then(context =>
                {
                    OnThen(context, context.Message.CorrelationId);
                    context.Saga.InvoiceCreated = true;
                    context.Saga.Url = context.Message.Url;
                })
                .TransitionTo(SendingInvoice)
                .Publish(context =>
                {
                    OnPublish(nameof(SendEmailCommandV1), context.Message.CorrelationId);
                    return new SendEmailCommandV1
                    {
                        EmailAddress = context.Saga.Email,
                        ContentFromUrl = context.Saga.Url,
                        CorrelationId = context.Message.CorrelationId
                    };
                }));

        During(SendingInvoice,
            When(EmailSent)
                .Then(context =>
                {
                    OnThen(context, context.Message.CorrelationId);
                    context.Saga.EmailId = context.Message.EmailId;
                })
                .TransitionTo(AwaitingPayment)
                .Finalize());
    }
}