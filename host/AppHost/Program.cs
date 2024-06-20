var builder = DistributedApplication.CreateBuilder(args);

var mongo = builder.AddMongoDB("Mongo");
var mongodb = mongo.AddDatabase("MongoDb", "orders-db");
var rabbitMq = builder.AddRabbitMQ("RabbitMq");

builder.AddProject<Projects.Debtors_Worker>("debtors-worker")
    .WithReference(rabbitMq);

builder.AddProject<Projects.Emails_Worker>("emails-worker")
    .WithReference(rabbitMq);

builder.AddProject<Projects.Invoices_Worker>("invoices-worker")
    .WithReference(rabbitMq)
    .WithReference(mongodb);

builder.AddProject<Projects.Invoices_Api>("invoices-api")
    .WithReference(rabbitMq)
    .WithReference(mongodb);

builder.Build().Run();