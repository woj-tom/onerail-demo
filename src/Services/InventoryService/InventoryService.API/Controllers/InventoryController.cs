using InventoryService.API.Contracts;
using InventoryService.Application.Handlers;
using Microsoft.AspNetCore.Mvc;

namespace InventoryService.API.Controllers;

[ApiController]
[Route("[controller]")]
public class InventoryController(InventoryCreateHandler handler) : ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] InventoryCreateReq request, CancellationToken ct)
    {
        // ToDo: Add validation
        await handler.Handle(new InventoryCreateCommand(
            request.ProductId, 
            request.Quantity,
            string.Empty), ct); // ToDo: Fix AddedBy after adding JWT auth
        
        return Created();
    }
}