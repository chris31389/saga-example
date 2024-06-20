using System;
using System.Threading.Tasks;

namespace Invoices.Worker.CreateInvoices;

public interface ICreateInvoiceService
{
    Task<Guid> Create(string currency, decimal amount);
}