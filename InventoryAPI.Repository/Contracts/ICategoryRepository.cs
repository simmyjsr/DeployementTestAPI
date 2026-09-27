using InventoryAPI.EntityModel;
using InventoryAPI.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace InventoryAPI.Repositories
{
    public interface ICategoryRepository
    {
        Task<IEnumerable<Category>> GetAllAsync();
        Task<Category?> GetByIdAsync(int id);
        Task<int> CreateAsync(Category category);
        Task<bool> UpdateAsync(Category category);
        Task<bool> DeleteAsync(int id);
    }
}
