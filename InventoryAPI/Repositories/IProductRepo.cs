using InventoryAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InventoryAPI.Repositories
{
    public interface IProductRepo
    {
        Task<IEnumerable<Product>> GetAllProductsAsync();
        Task<IEnumerable<Brand>> GetAllBrand();
        Task<Product> GetProductByIdAsync(int productId);
        Task<int> AddProductAsync(viewProduct product);
        Task<bool> UpdateProductAsync(viewProduct product);
        Task<bool> DeleteProductAsync(int productId);     
        Task<IEnumerable<Product>> SearchProductsAsync(string searchText);
    }
}
