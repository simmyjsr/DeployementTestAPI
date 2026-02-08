using InventoryAPI.EntityModel;
using InventoryAPI.Repositories;

namespace InventoryAPI.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly ICategoryRepository _repo;
        public CategoryService(ICategoryRepository repo)
        {
            _repo = repo;
        }
        public Task<IEnumerable<Category>> GetAllAsync() => _repo.GetAllAsync();

        public Task<Category?> GetByIdAsync(int id) => _repo.GetByIdAsync(id);

        public Task<int> CreateAsync(Category category)
        {
            category.CreatedAt = DateTime.UtcNow;
            return _repo.CreateAsync(category);
        }

        public Task<bool> UpdateAsync(Category category) => _repo.UpdateAsync(category);

        public Task<bool> DeleteAsync(int id) => _repo.DeleteAsync(id);
    }
}
