using Dapper;
using InventoryAPI.Data;
using InventoryAPI.EntityModel;
using InventoryAPI.Models;

namespace InventoryAPI.Repositories
{
    public class CategoryRepository : ICategoryRepository
    {
        private readonly DapperContext _context;

        public CategoryRepository(DapperContext context)
        {
            _context = context;
        }

        public async Task<int> CreateAsync(Category category)
        {
            using var db = _context.CreateConnection();
            var sql = @"INSERT INTO Category (CategoryName, Description, CreatedAt)
                    VALUES (@CategoryName, @Description, @CreatedAt);
                    SELECT CAST(SCOPE_IDENTITY() as int);";

            return await db.ExecuteScalarAsync<int>(sql, category);
        }

        public async Task<bool> DeleteAsync(int id)
        {
            using var db = _context.CreateConnection();
            var sql = "DELETE FROM Categories WHERE CategoryID = @Id";
            var affected = await db.ExecuteAsync(sql, new { Id = id });
            return affected > 0;
        }

        public async Task<IEnumerable<Category>> GetAllAsync()
        {
            using var db = _context.CreateConnection();
            var sql = "select CategoryID,CategoryName from Categories";
            return await db.QueryAsync<Category>(sql);
        }

        public async Task<Category?> GetByIdAsync(int id)
        {
            using var db = _context.CreateConnection();
            var sql = "SELECT * FROM Category WHERE CategoryID = @Id";
            return await db.QueryFirstOrDefaultAsync<Category>(sql, new { Id = id });
        }

        public async Task<bool> UpdateAsync(Category category)
        {
            using var db = _context.CreateConnection();
            var sql = @"UPDATE Categories
                    SET CategoryName = @CategoryName,
                        Description = @Description
                    WHERE CategoryID = @CategoryID";

            var affected = await db.ExecuteAsync(sql, category);
            return affected > 0;
        }
    }
}
