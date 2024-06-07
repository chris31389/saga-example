using System;
using System.Threading.Tasks;
using Contracts.RaiseInvoiceCommand.V1;
using FluentAssertions;
using MassTransit;
using MassTransit.Testing;
using Microsoft.Extensions.DependencyInjection;
using RaiseInvoices.Worker.Consumers;
using Xunit;

namespace RaiseInvoices.Worker.ComponentTests
{
    public class ConsumerTests
    {
        [Fact]
        public async Task When_ConsumerConsumesMessage_ThenMessageIsLogged()
        {
            // Arrange
            await using var provider = new ServiceCollection()
                .AddMassTransitTestHarness(cfg => cfg
                    .AddConsumer<RaiseInvoiceCommandV1Consumer>())
                .BuildServiceProvider(true);

            var harness = provider.GetRequiredService<ITestHarness>();
            await harness.Start();

            try
            {
                var bus = provider.GetRequiredService<IBus>();
                var message = new RaiseInvoiceCommandV1
                {
                    DebtorId = Guid.NewGuid().ToString(),
                    Currency = "GBP",
                    Value = 24.99m
                };

                // Act
                await bus.Publish(message);

                // Assert
                var result = await harness.Consumed.Any<RaiseInvoiceCommandV1>();
                result.Should().BeTrue();
            }
            finally
            {
                await harness.Stop();
            }
        }
    }
}