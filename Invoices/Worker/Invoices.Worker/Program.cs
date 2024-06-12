using System;
using System.Threading.Tasks;
using Invoices.Worker.CreateInvoices;
using Invoices.Worker.Sagas;
using Invoices.Worker.Shared.Persistence;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Invoices.Worker;

public class Program
{
    public static async Task Main(string[] args)
    {
        var app = CreateHostBuilder(args).Build();
        await MigrateDatabase(app);
        await app.RunAsync();
    }

    public static IHostBuilder CreateHostBuilder(string[] args) => Host
        .CreateDefaultBuilder(args)
        .ConfigureServices((hostContext, services) => services
            .AddCreateInvoiceFeature()
            .AddDbContext<InvoiceDbContext>(options =>
                options.UseNpgsql(hostContext.Configuration.GetConnectionString("Postgres")))
            .AddMassTransit(busConfigurator =>
            {
                busConfigurator.SetKebabCaseEndpointNameFormatter();
                busConfigurator.AddConsumers(typeof(Program).Assembly);
                busConfigurator.AddSagaStateMachine<OrderSaga, OrderSagaData>()
                    .EntityFrameworkRepository(efConfigurator =>
                    {
                        efConfigurator.ExistingDbContext<InvoiceDbContext>();
                        efConfigurator.UsePostgres();
                    });
                busConfigurator.UsingRabbitMq((context, cfg) =>
                {
                    cfg.Host(new Uri(hostContext.Configuration.GetConnectionString("RabbitMq")!), hst =>
                    {
                        hst.Username("guest");
                        hst.Password("guest");
                    });
                    cfg.UseInMemoryOutbox(context);
                    cfg.ConfigureEndpoints(context);
                });
            })
        );

    private static async Task MigrateDatabase(IHost app)
    {
        using var scope = app.Services.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<InvoiceDbContext>();
        await context.Database.MigrateAsync();
    }
}