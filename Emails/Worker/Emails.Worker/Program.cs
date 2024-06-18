using System;
using System.Threading.Tasks;
using MassTransit;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;

namespace Emails.Worker;

public class Program
{
    public static async Task Main(string[] args) => await CreateHostBuilder(args).Build().RunAsync();

    public static IHostBuilder CreateHostBuilder(string[] args) => Host
        .CreateDefaultBuilder(args)
        .ConfigureServices((hostContext, services) =>
            services.AddMassTransit(x =>
            {
                x.SetKebabCaseEndpointNameFormatter();
                x.UsingRabbitMq((context, cfg) =>
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
}