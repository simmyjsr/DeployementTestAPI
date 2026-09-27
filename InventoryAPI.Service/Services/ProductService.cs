using Azure.Core;
using InventoryAPI.Models;
using InventoryAPI.Repositories;
using static InventoryAPI.Helpers.CommonMethods;

namespace InventoryAPI.Infrastructure.Services
{
    public class ProductService : IProductService
    {
        private readonly IProductRepo _repository;

        public ProductService(IProductRepo repository)
        {
            _repository = repository;
        }

        public async Task<IEnumerable<Product>> GetAllProductsAsync() => await _repository.GetAllProductsAsync();

        public async Task<IEnumerable<Brand>> GetAllBrand() => await _repository.GetAllBrand();
        public async Task<Product> GetProductByIdAsync(int id) => await _repository.GetProductByIdAsync(id);

        public async Task<int> CreateProductAsync(CreateProductRequest request)
        {
            if (request == null)
                throw new ArgumentNullException(nameof(request));
            var product = new viewProduct
            {
                ProductID = Guid.NewGuid(),
                ProductName = request.ProductName,
                Description = request.Description,
                Price = request.Price,
                StockQuantity = request.StockQuantity,
                CategoryID = request.CategoryID,
                Feature = request.Feature,
                SupplierID = request.SupplierID,
                CreatedAt = DateTime.UtcNow,
                CreatedBy = "seema",
                ProductNumber = ProductNumberGenerator.Generate()
            };

            if (request.Image != null)
            {
                // request.Image previously was IFormFile; after refactor domain uses byte[] for image.
                // If Image is IFormFile keep existing behavior; if it is byte[] assign directly.
                if (request.Image is byte[] bytes)
                {
                    product.ImageBytes = bytes;
                }
                else
                {
                    using var memoryStream = new MemoryStream();
                    // try to dynamically handle IFormFile-like types if present
                    var copyToAsync = request.Image?.GetType().GetMethod("CopyToAsync");
                    if (copyToAsync != null)
                    {
                        // assume it's IFormFile-like; call CopyToAsync via reflection
                        var task = (System.Threading.Tasks.Task)copyToAsync.Invoke(request.Image, new object[] { memoryStream, default(System.Threading.CancellationToken) });
                        await task;
                        product.ImageBytes = memoryStream.ToArray();
                    }
                    else if (request.Image is System.IO.Stream s)
                    {
                        await s.CopyToAsync(memoryStream);
                        product.ImageBytes = memoryStream.ToArray();
                    }
                }
            }
            return await _repository.AddProductAsync(product);
         }

        public async Task<bool> UpdateProductAsync(viewProduct product)
        {
            product.UpdatedAt = DateTime.UtcNow;
            product.UpdatedBy = "seema";


            return  await _repository.UpdateProductAsync(product);
        }

        public async Task<bool> RemoveProductAsync(int id) => await _repository.DeleteProductAsync(id);



        public async Task<IEnumerable<Product>> SearchProductsAsync(string search)
        {
            // Business logic can be added here if needed
            return await _repository.SearchProductsAsync(search);
        }
    }
}
