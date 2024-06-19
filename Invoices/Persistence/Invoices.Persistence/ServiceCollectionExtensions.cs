﻿using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Driver;

namespace Invoices.Persistence;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddInvoicesPersistence(this IServiceCollection services)
    {
        services.AddSingleton<IMongoClient>(provider =>
        {
            var configuration = provider.GetRequiredService<IConfiguration>();
            var connectionString = configuration.GetConnectionString("Mongo");
            return new MongoClient(connectionString);
        });
        services.AddSingleton<IMongoDatabase>(provider =>
        {
            var mongoClient = provider.GetRequiredService<IMongoClient>();
            var configuration = provider.GetRequiredService<IConfiguration>();
            var connectionString = configuration.GetConnectionString("Mongo");
            var mongoUrl = new MongoUrl(connectionString);
            var databaseName = !string.IsNullOrWhiteSpace(mongoUrl.DatabaseName)
                ? mongoUrl.DatabaseName
                : "orders-db";

            return mongoClient.GetDatabase(databaseName);
        });

        return services;
    }
}