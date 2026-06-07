using System.Collections.Generic;
using System.Threading.Tasks;

namespace InventoryAPI.Infrastructure.Repositories
{
    using InventoryAPI.Data;
    using Dapper;
    using System.Data;
    using System.Collections.Generic;

    public interface IGenericRepository<T>
    {
        Task<T?> GetByIdAsync(object id);
        Task<IEnumerable<T>> GetAllAsync();
        Task<int> AddAsync(T entity);
        Task<int> UpdateAsync(T entity);
        Task<int> DeleteAsync(object id);
    }

    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        private readonly DapperContext _context;

        public GenericRepository(DapperContext context)
        {
            _context = context;
        }

        public async Task<int> AddAsync(T entity)
        {
            // Implementation depends on concrete SQL and mapping.
            // This is a stub to illustrate usage. Prefer specific repositories per aggregate.
            throw new NotImplementedException();
        }

        public async Task<int> DeleteAsync(object id)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<T>> GetAllAsync()
        {
            using var conn = _context.CreateConnection();
            // Needs concrete SQL - require specific repo
            throw new NotImplementedException();
        }

        public async Task<T?> GetByIdAsync(object id)
        {
            using var conn = _context.CreateConnection();
            throw new NotImplementedException();
        }

        public async Task<int> UpdateAsync(T entity)
        {
            throw new NotImplementedException();
        }
    }
}
