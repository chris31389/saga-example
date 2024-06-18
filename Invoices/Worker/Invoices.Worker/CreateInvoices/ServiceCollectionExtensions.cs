using Invoices.Worker.Sagas;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Bson.Serialization;

namespace Invoices.Worker.CreateInvoices;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddCreateInvoiceFeature(this IServiceCollection services) => services
        .AddTransient<ICreateInvoiceService, CreateInvoiceService>();
        // .AddSingleton<BsonClassMap<OrderSagaData>, OrderSagaDataClassMap>();
}