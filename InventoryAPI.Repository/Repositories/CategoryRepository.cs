using Dapper;
using InventoryAPI.Models;
using InventoryAPI.Data;
using Microsoft.EntityFrameworkCore;

namespace InventoryAPI.Repositories
{
    // Hybrid repository: EF Core for CRUD, Dapper for a hot query
    public class CategoryRepository : ICategoryRepository
    {
        private readonly AppDbContext _db;

        public CategoryRepository(AppDbContext db)
        {
            _db = db;
        }

        public async Task<int> CreateAsync(Category category)
        {
            _db.Categories.Add(category);
            await _db.SaveChangesAsync();
            return category.CategoryID;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var cat = await _db.Categories.FindAsync(id);
            if (cat == null) return false;
            _db.Categories.Remove(cat);
            var changed = await _db.SaveChangesAsync();
            return changed > 0;
        }

        public async Task<IEnumerable<Category>> GetAllAsync()
        {
            return await _db.Categories.AsNoTracking().ToListAsync();
        }

        public async Task<Category?> GetByIdAsync(int id)
        {
            return await _db.Categories.AsNoTracking().FirstOrDefaultAsync(c => c.CategoryID == id);
        }

        public async Task<bool> UpdateAsync(Category category)
        {
            _db.Categories.Update(category);
            var changed = await _db.SaveChangesAsync();
            return changed > 0;
        }

        // Example of a hot query kept in Dapper for performance: get category product counts
        public async Task<IEnumerable<CategoryProductCount>> GetCategoryProductCounts()
        {
            const string sql = @"SELECT c.CategoryID, c.CategoryName, COUNT(p.ProductID) AS ProductCount
                                 FROM Categories c
                                 LEFT JOIN Products p ON p.CategoryID = c.CategoryID
                                 GROUP BY c.CategoryID, c.CategoryName";

            var conn = _db.Database.GetDbConnection();
            // Ensure connection is open for Dapper
            if (conn.State != System.Data.ConnectionState.Open)
                await conn.OpenAsync();

            var result = await conn.QueryAsync<CategoryProductCount>(sql);
            return result;
        }

        public record CategoryProductCount(int CategoryID, string CategoryName, int ProductCount);
    }
}
