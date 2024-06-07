using System.Threading.Tasks;
using Contracts.CreateOrUpdateDebtorCommand.V1;
using MassTransit;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Invoices.Api.Controllers;

[ApiController]
[Route("invoice")]
public class InvoiceController(ILogger<InvoiceController> logger, IBus bus) : ControllerBase
{
    [HttpPost(Name = "StartInvoiceCreation")]
    public async Task<IActionResult> StartInvoiceCreation([FromBody] StartInvoiceRequestModel model)
    {
        logger.LogInformation("Starting an invoice creation");
        await bus.Publish(new CreateOrUpdateDebtorCommandV1
        {
            Name = model.Name,
        });

        return Accepted();
    }
}