using FluentValidation;
using InventoryAPI.Models;

namespace InventoryAPI.Features.Products.Validators
{
    public class CreateProductValidator : AbstractValidator<viewProduct>
    {
        public CreateProductValidator()
        {
            RuleFor(x => x.ProductName).NotEmpty().MaximumLength(200);
            RuleFor(x => x.Price).GreaterThanOrEqualTo(0);
            RuleFor(x => x.StockQuantity).GreaterThanOrEqualTo(0);
        }
    }
}
