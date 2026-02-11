using Microsoft.AspNetCore.Mvc;
using ProductService.API.Contracts;
using ProductService.Application.Handlers;

namespace ProductService.API.Controllers;

[ApiController]
[Route("[controller]")]
public class ProductController(
    ProductListHandler listHandler,
    ProductCreateHandler createHandler) : ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] ProductCreateReq request, CancellationToken ct)
    {
        // ToDo: Add validation
        await createHandler.HandleAsync(new ProductCreateCommand(
            request.Name,
            request.Description,
            request.Price), ct);
        return Created();
    }

    [HttpGet]
    public async Task<IActionResult> List(CancellationToken ct)
    {
        var list = await listHandler.HandleAsync(ct);
        return Ok(list);
    }
}