using System;
using System.Threading.Tasks;
using Debtors.Messages.CreateOrUpdateDebtorCommand.V1;
using Debtors.Worker.Consumers;
using FluentAssertions;
using MassTransit;
using MassTransit.Testing;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace Debtors.Worker.ComponentTests
{
    public class ConsumerTests
    {
        [Fact]
        public async Task When_ConsumerConsumesMessage_ThenMessageIsLogged()
        {
            // Arrange
            await using var provider = new ServiceCollection()
                .AddMassTransitTestHarness(cfg => cfg
                    .AddConsumer<CreateOrUpdateDebtorCommandV1Consumer>())
                .BuildServiceProvider(true);

            var harness = provider.GetRequiredService<ITestHarness>();
            await harness.Start();

            try
            {
                var bus = provider.GetRequiredService<IBus>();
                var message = new CreateOrUpdateDebtorCommandV1
                {
                    Name = "Chris",
                    CustomerId = Guid.NewGuid().ToString(),
                    CorrelationId = Guid.NewGuid(),
                    Email = Guid.NewGuid().ToString()
                };

                // Act
                await bus.Publish(message);

                // Assert
                var result = await harness.Consumed.Any<CreateOrUpdateDebtorCommandV1>();
                result.Should().BeTrue();
            }
            finally
            {
                await harness.Stop();
            }
        }
    }
}