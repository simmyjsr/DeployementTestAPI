using InventoryAPI.Models;

namespace InventoryAPI.Services
{
    public interface IProductService
    {
        Task<IEnumerable<Product>> GetAllProductsAsync();
        Task<IEnumerable<Brand>> GetAllBrand();
        Task<Product> GetProductByIdAsync(int id);
        Task<int> CreateProductAsync(CreateProductRequest product);
        Task<bool> UpdateProductAsync(viewProduct product);
        Task<bool> RemoveProductAsync(int id);
        Task<IEnumerable<Product>> SearchProductsAsync(string ? search);

    }
}