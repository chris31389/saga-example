using System;
using System.Threading.Tasks;

namespace Invoices.Worker.CreateInvoices;

public class CreateInvoiceService : ICreateInvoiceService
{
    public async Task<Guid> Create(string currency, decimal amount)
    {
        // Create new Invoice
        var invoiceId = Guid.NewGuid();
        return await Task.FromResult(invoiceId);
    }
}