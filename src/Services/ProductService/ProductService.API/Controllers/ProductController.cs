using FluentValidation;
using Microsoft.AspNetCore.Authorization;
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
    [Authorize(Roles = "write")]
    public async Task<IActionResult> Create(
        [FromBody] ProductCreateReq request,
        IValidator<ProductCreateReq> validator,
        CancellationToken ct)
    {
        await validator.ValidateAndThrowAsync(request, ct);
        
        await createHandler.HandleAsync(new ProductCreateCommand(
            request.Name,
            request.Description,
            request.Price), ct);
        return Created();
    }

    [HttpGet]
    [Authorize(Roles = "read")]
    public async Task<IActionResult> List(CancellationToken ct)
    {
        var list = await listHandler.HandleAsync(ct);
        return Ok(list);
    }
}