using System;
using Contracts.RaiseInvoiceCommand.V1;
using Contracts.RaiseInvoiceCompleted.V1;
using Contracts.SendEmailCommand.V1;
using Debtors.Messages.CreateOrUpdateDebtorCommand.V1;
using Debtors.Messages.CreateOrUpdateDebtorCompleted.V1;
using Invoices.Messages.OrderStartedV1.V1;
using MassTransit;

namespace Invoices.Worker.Sagas;

public class OrderSaga : MassTransitStateMachine<OrderSagaData>
{
    public State CreatingOrUpdatingDebtor { get; set; }
    public State RaisingInvoice { get; set; }
    public State SendingInvoice { get; set; }
    public Event<OrderStartedV1> OrderStarted { get; set; }
    public Event<CreateOrUpdateDebtorCompletedV1> CreateOrUpdateDebtorCompleted { get; set; }
    public Event<RaiseInvoiceCompletedV1> RaiseInvoiceCompleted { get; set; }

    public OrderSaga()
    {
        InstanceState(x => x.CurrentState);

        // Consider changing the correlation id to a transaction Id so that we can have multiple invoices for a debtor
        Event(() => OrderStarted, e => e.CorrelateById(m => m.Message.OrderId));
        Event(() => CreateOrUpdateDebtorCompleted, e => e.CorrelateById(m => m.Message.OrderId));
        Event(() => RaiseInvoiceCompleted, e => e.CorrelateById(m => m.Message.OrderId));

        Initially(
            When(OrderStarted)
                .Then(context =>
                {
                    context.Saga.DebtorId = null;
                    context.Saga.CorrelationId = Guid.NewGuid();
                    context.Saga.Name = context.Message.Name;
                    context.Saga.Amount = context.Message.Amount;
                    context.Saga.Currency = context.Message.Currency;
                    context.Saga.DebtorEmail = context.Message.Email;
                })
                .TransitionTo(CreatingOrUpdatingDebtor)
                .Publish(context => new CreateOrUpdateDebtorCommandV1
                {
                    CustomerId = context.Message.CustomerId,
                    Name = context.Message.Name,
                    OrderId = context.Message.OrderId,
                    Email = context.Message.Email,
                }));

        During(CreatingOrUpdatingDebtor,
            When(CreateOrUpdateDebtorCompleted)
                .Then(context =>
                {
                    context.Saga.DebtorCreated = true;
                    context.Saga.DebtorId = context.Message.DebtorId;
                    context.Saga.DebtorEmail = context.Message.DebtorEmail;
                })
                .TransitionTo(RaisingInvoice)
                .Publish(context => new RaiseInvoiceCommandV1
                {
                    OrderId = context.Message.OrderId,
                    DebtorId = context.Saga.DebtorId!.Value,
                    Currency = context.Saga.Currency,
                    Value = context.Saga.Amount
                }));

        During(RaisingInvoice,
            When(RaiseInvoiceCompleted)
                .Then(context =>
                {
                    context.Saga.InvoiceCreated = true;
                    context.Saga.Url = context.Message.Url;
                })
                .TransitionTo(SendingInvoice)
                .Publish(context => new SendEmailCommandV1
                {
                    EmailAddress = context.Saga.DebtorEmail,
                    ContentFromUrl = context.Saga.Url
                })
                .Finalize());
    }
}