using MediatR;
using InventoryAPI.Models;

namespace InventoryAPI.Features.Products.Commands
{
    public record CreateProductCommand(viewProduct Product) : IRequest<int>;
}
