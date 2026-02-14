using FluentValidation;
using InventoryService.API.Contracts;
using InventoryService.Domain.Repositories;

namespace InventoryService.API.Validators;

public class InventoryCreateReqValidator
    : AbstractValidator<InventoryCreateReq>
{
    public InventoryCreateReqValidator(IProductRepository productRepository)
    {
        RuleFor(x => x.Quantity)
            .GreaterThan(0)
            .WithMessage("Quantity must be greater than 0.");

        RuleFor(x => x.ProductId)
            .MustAsync(async (productId, cancellationToken) 
                => await productRepository.GetAsync(productId, cancellationToken) is not null)
            .WithMessage("Product does not exist.");
    }
}