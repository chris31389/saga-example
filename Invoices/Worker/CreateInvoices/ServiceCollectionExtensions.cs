using Microsoft.Extensions.DependencyInjection;

namespace Invoices.Worker.CreateInvoices;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddCreateInvoiceFeature(this IServiceCollection services) => services
        .AddTransient<ICreateInvoiceService, CreateInvoiceService>();
}