using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using FluentValidation;
using InventoryService.API.Contracts;
using InventoryService.Application.Handlers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace InventoryService.API.Controllers;

[ApiController]
[Route("[controller]")]
public class InventoryController(InventoryCreateHandler handler) : ControllerBase
{
    [HttpPost]
    [Authorize(Roles = "write")]
    public async Task<IActionResult> Create(
        [FromBody] InventoryCreateReq request, 
        IValidator<InventoryCreateReq> validator,
        CancellationToken ct)
    {
        await validator.ValidateAndThrowAsync(request, ct);

        var addedBy = User.FindFirstValue(JwtRegisteredClaimNames.Sub)
                      ?? "Unrecognized";
        
        await handler.HandleAsync(new InventoryCreateCommand(
            request.ProductId, 
            request.Quantity,
            addedBy), ct);
        
        return Created();
    }
}