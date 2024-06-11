using System;
using System.Threading.Tasks;
using Emails.Messages.SendEmailCommand.V1;
using Emails.Worker.Consumers;
using FluentAssertions;
using MassTransit;
using MassTransit.Testing;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace Emails.Worker.ComponentTests
{
    public class ConsumerTests
    {
        [Fact]
        public async Task When_ConsumerConsumesMessage_ThenMessageIsLogged()
        {
            // Arrange
            await using var provider = new ServiceCollection()
                .AddMassTransitTestHarness(cfg => cfg
                    .AddConsumer<SendEmailCommandV1Consumer>())
                .BuildServiceProvider(true);

            var harness = provider.GetRequiredService<ITestHarness>();
            await harness.Start();

            try
            {
                var bus = provider.GetRequiredService<IBus>();
                var message = new SendEmailCommandV1
                {
                    ContentFromUrl = "https://hello.com",
                    EmailAddress = "my@email.com",
                    CorrelationId = Guid.NewGuid()
                };

                // Act
                await bus.Publish(message);

                // Assert
                var result = await harness.Consumed.Any<SendEmailCommandV1>();
                result.Should().BeTrue();
            }
            finally
            {
                await harness.Stop();
            }
        }
    }
}