using System;
using System.Threading.Tasks;
using MassTransit;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;

namespace Debtors.Worker;

public class Program
{
    public static async Task Main(string[] args) => await CreateHostBuilder(args).Build().RunAsync();

    public static IHostBuilder CreateHostBuilder(string[] args) => Host
        .CreateDefaultBuilder(args)
        .ConfigureServices((hostContext, services) =>
            services.AddMassTransit(x =>
            {
                x.SetKebabCaseEndpointNameFormatter();
                x.AddConsumers(typeof(Program).Assembly);
                x.UsingRabbitMq((context, cfg) =>
                {
                    cfg.Host(new Uri(hostContext.Configuration.GetConnectionString("RabbitMq")!), hst =>
                    {
                        hst.Username("guest");
                        hst.Password("guest");
                    });
                    cfg.ConfigureEndpoints(context);
                });
            })
        );
}