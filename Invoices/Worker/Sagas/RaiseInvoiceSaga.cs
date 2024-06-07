using Contracts.CreateOrUpdateDebtorCompleted.V1;
using Contracts.RaiseInvoiceCompleted.V1;
using MassTransit;

namespace Invoices.Worker.Sagas;

public class RaiseInvoiceSaga : MassTransitStateMachine<RaiseInvoiceSagaData>
{
    public State CreatingOrUpdatingDebtor { get; set; }
    public State RaisingInvoice { get; set; }
    public Event<CreateOrUpdateDebtorCompletedV1> CreateOrUpdateDebtorCompleted { get; set; }
    public Event<RaiseInvoiceCompletedV1> RaiseInvoiceCompleted { get; set; }

    public RaiseInvoiceSaga()
    {
        InstanceState(x => x.CurrentState);

        // Consider changing the correlation id to a transaction Id so that we can have multiple invoices for a debtor
        Event(() => CreateOrUpdateDebtorCompleted, e => e.CorrelateById(m => m.Message.DebtorId));
        Event(() => RaiseInvoiceCompleted, e => e.CorrelateById(m => m.Message.DebtorId));

        Initially(
            When(CreateOrUpdateDebtorCompleted)
                .Then(context =>
                {
                    context.Saga.DebtorId = context.Message.DebtorId;
                })
                .TransitionTo(CreatingOrUpdatingDebtor)
                .Publish(context => new ));
    }
}