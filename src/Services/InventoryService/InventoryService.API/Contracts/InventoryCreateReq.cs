namespace InventoryService.API.Contracts;

public sealed record InventoryCreateReq(Guid ProductId, int Quantity);