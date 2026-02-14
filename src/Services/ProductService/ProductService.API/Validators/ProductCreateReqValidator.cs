using FluentValidation;
using ProductService.API.Contracts;

namespace ProductService.API.Validators;

public class ProductCreateReqValidator : AbstractValidator<ProductCreateReq>
{
    public  ProductCreateReqValidator()
    {
        RuleFor(x => x.Name)
            .NotNull()
            .NotEmpty();

        RuleFor(x => x.Price)
            .GreaterThan(0);
    }
}