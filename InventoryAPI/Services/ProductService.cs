
using Azure.Core;
using InventoryAPI.Models;
using InventoryAPI.Repositories;
using static InventoryAPI.Helpers.CommonMethods;


namespace InventoryAPI.Services
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
                using var memoryStream = new MemoryStream();
                await request.Image.CopyToAsync(memoryStream);
                product.ImageBytes = memoryStream.ToArray(); // Convert image to byte[]
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

