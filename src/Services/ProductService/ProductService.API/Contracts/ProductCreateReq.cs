namespace ProductService.API.Contracts;

public sealed record ProductCreateReq(string Name, string Description, decimal Price);