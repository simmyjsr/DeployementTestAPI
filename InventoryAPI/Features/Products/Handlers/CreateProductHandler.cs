using MediatR;
using InventoryAPI.Repositories;

namespace InventoryAPI.Features.Products.Handlers
{
    public class CreateProductHandler : IRequestHandler<Commands.CreateProductCommand, int>
    {
        private readonly IProductRepo _productRepo;

        public CreateProductHandler(IProductRepo productRepo)
        {
            _productRepo = productRepo;
        }

        public async Task<int> Handle(Commands.CreateProductCommand request, CancellationToken cancellationToken)
        {
            var product = request.Product;
            // delegate to repository
            var result = await _productRepo.AddProductAsync(product);
            return result;
        }
    }
}
