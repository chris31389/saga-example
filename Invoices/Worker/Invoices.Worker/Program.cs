﻿using System;
using Invoices.Persistence;
using Invoices.Worker.CreateInvoices;
using Invoices.Worker.Sagas;
using MassTransit;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using MongoDB.Driver;

var builder = Host.CreateApplicationBuilder(args);
builder.Services.Configure<MassTransitHostOptions>(options => { options.WaitUntilStarted = true; });
builder.Services
    .AddCreateInvoiceFeature()
    .AddInvoicesPersistence("MongoDb")
    .AddMassTransit(busConfigurator =>
    {
        busConfigurator.SetKebabCaseEndpointNameFormatter();
        busConfigurator.AddConsumers(typeof(Program).Assembly);
        busConfigurator
            .AddSagaStateMachine<OrderSaga, OrderSagaData>()
            .MongoDbRepository(r =>
            {
                r.DatabaseFactory(provider => provider.GetRequiredService<IMongoDatabase>());
                r.CollectionName = Constants.CollectionName;
            });
        busConfigurator.UsingRabbitMq((context, cfg) =>
        {
            cfg.Host(new Uri(builder.Configuration.GetConnectionString("RabbitMq")!));
            cfg.UseInMemoryOutbox(context);
            cfg.ConfigureEndpoints(context);
        });
    });

var host = builder.Build();
await host.RunAsync();