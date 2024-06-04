using Microsoft.AspNetCore.Mvc;

namespace Invoice.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class InvoiceController(ILogger<InvoiceController> logger) : ControllerBase
{
    [HttpPost(Name = "StartInvoiceCreation")]
    public async Task<IActionResult> StartInvoiceCreation()
    {
        logger.LogInformation("Starting an invoice creation");

        await Task.Delay(100);
        return Accepted();
    }
}