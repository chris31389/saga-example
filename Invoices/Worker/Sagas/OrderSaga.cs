using Debtors.Messages.CreateOrUpdateDebtorCommand.V1;
using Debtors.Messages.CreateOrUpdateDebtorCompleted.V1;
using Emails.Messages.EmailSent.V1;
using Emails.Messages.SendEmailCommand.V1;
using Invoices.Messages.OrderCreated.V1;
using Invoices.Messages.RaiseInvoiceCommand.V1;
using Invoices.Messages.RaiseInvoiceCompleted.V1;
using MassTransit;

namespace Invoices.Worker.Sagas;

public class OrderSaga : MassTransitStateMachine<OrderSagaData>
{
    public State CreatingOrUpdatingDebtor { get; set; }
    public State RaisingInvoice { get; set; }
    public State SendingInvoice { get; set; }
    public State AwaitingPayment { get; set; }
    public Event<OrderCreatedV1> OrderCreated { get; set; }
    public Event<CreateOrUpdateDebtorCompletedV1> CreateOrUpdateDebtorCompleted { get; set; }
    public Event<RaiseInvoiceCompletedV1> RaiseInvoiceCompleted { get; set; }
    public Event<EmailSentV1> EmailSent { get; set; }

    public OrderSaga()
    {
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
                    context.Saga.OrderId = context.Message.OrderId;
                    context.Saga.CorrelationId = context.Message.CorrelationId;
                    context.Saga.Name = context.Message.Name;
                    context.Saga.Amount = context.Message.Amount;
                    context.Saga.Currency = context.Message.Currency;
                    context.Saga.Email = context.Message.Email;
                })
                .TransitionTo(CreatingOrUpdatingDebtor)
                .Publish(context => new CreateOrUpdateDebtorCommandV1
                {
                    CustomerId = context.Message.CustomerId,
                    Name = context.Message.Name,
                    CorrelationId = context.Message.OrderId,
                    Email = context.Message.Email,
                }));

        During(CreatingOrUpdatingDebtor,
            When(CreateOrUpdateDebtorCompleted)
                .Then(context =>
                {
                    context.Saga.DebtorCreated = true;
                    context.Saga.DebtorId = context.Message.DebtorId;
                })
                .TransitionTo(RaisingInvoice)
                .Publish(context => new RaiseInvoiceCommandV1
                {
                    CorrelationId = context.Message.CorrelationId,
                    DebtorId = context.Saga.DebtorId,
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
                    EmailAddress = context.Saga.Email,
                    ContentFromUrl = context.Saga.Url
                }));

        During(SendingInvoice,
            When(EmailSent)
            .Then(context =>
                {
                    context.Saga.EmailId = context.Message.EmailId;
                })
                .TransitionTo(AwaitingPayment)
                .Finalize());
    }
}