using Dapper;
using InventoryAPI.Data;
using InventoryAPI.EntityModel;
using InventoryAPI.Models;

namespace InventoryAPI.Repositories
{
    public class CategoryRepository : ICategoryRepository
    {
        private readonly DapperContext _context;
        // Use consistent table name 'Categories' and centralize SQL statements
        private const string TableName = "Categories";
        private const string SqlInsert = @"INSERT INTO Categories (CategoryName, Description, CreatedAt)
                    VALUES (@CategoryName, @Description, @CreatedAt);
                    SELECT CAST(SCOPE_IDENTITY() as int);";
        private const string SqlDelete = "DELETE FROM Categories WHERE CategoryID = @Id";
        private const string SqlGetAll = "SELECT CategoryID, CategoryName, Description, CreatedAt FROM Categories";
        private const string SqlGetById = "SELECT CategoryID, CategoryName, Description, CreatedAt FROM Categories WHERE CategoryID = @Id";
        private const string SqlUpdate = @"UPDATE Categories
                    SET CategoryName = @CategoryName,
                        Description = @Description
                    WHERE CategoryID = @CategoryID";

        public CategoryRepository(DapperContext context)
        {
            _context = context;
        }

        public async Task<int> CreateAsync(Category category)
        {
            using var db = _context.CreateConnection();
            return await db.ExecuteScalarAsync<int>(SqlInsert, category);
        }

        public async Task<bool> DeleteAsync(int id)
        {
            using var db = _context.CreateConnection();
            var affected = await db.ExecuteAsync(SqlDelete, new { Id = id });
            return affected > 0;
        }

        public async Task<IEnumerable<Category>> GetAllAsync()
        {
            using var db = _context.CreateConnection();
            return await db.QueryAsync<Category>(SqlGetAll);
        }

        public async Task<Category?> GetByIdAsync(int id)
        {
            using var db = _context.CreateConnection();
            return await db.QueryFirstOrDefaultAsync<Category>(SqlGetById, new { Id = id });
        }

        public async Task<bool> UpdateAsync(Category category)
        {
            using var db = _context.CreateConnection();
            var affected = await db.ExecuteAsync(SqlUpdate, category);
            return affected > 0;
        }
    }
}
