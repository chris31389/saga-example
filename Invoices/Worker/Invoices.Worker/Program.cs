using System;
using Invoices.Worker.CreateInvoices;
using Invoices.Worker.Sagas;
using MassTransit;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;

var builder = Host.CreateApplicationBuilder(args);
builder.Services
    .AddCreateInvoiceFeature()
    .AddMassTransit(busConfigurator =>
    {
        busConfigurator.SetKebabCaseEndpointNameFormatter();
        busConfigurator.AddConsumers(typeof(Program).Assembly);
        busConfigurator
            .AddSagaStateMachine<OrderSaga, OrderSagaData>()
            .MongoDbRepository(builder.Configuration.GetConnectionString("Mongo"), r =>
            {
                r.DatabaseName = "orders-db";
                r.CollectionName = "orders";
            });
        busConfigurator.UsingRabbitMq((context, cfg) =>
        {
            cfg.Host(new Uri(builder.Configuration.GetConnectionString("RabbitMq")!), hst =>
            {
                hst.Username("guest");
                hst.Password("guest");
            });
            cfg.UseInMemoryOutbox(context);
            cfg.ConfigureEndpoints(context);
        });
    });

var host = builder.Build();
await host.RunAsync();