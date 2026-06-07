using System.Collections.Generic;
using System.Threading.Tasks;

namespace InventoryAPI.Infrastructure.Repositories
{
    // Simple in-memory placeholder implementation. Replace with Dapper/EF implementation.
    public class GenericRepositoryImplementation<T> : IGenericRepository<T>
    {
        public Task<int> AddAsync(T entity)
        {
            throw new System.NotImplementedException();
        }

        public Task<int> DeleteAsync(object id)
        {
            throw new System.NotImplementedException();
        }

        public Task<IEnumerable<T>> GetAllAsync()
        {
            throw new System.NotImplementedException();
        }

        public Task<T?> GetByIdAsync(object id)
        {
            throw new System.NotImplementedException();
        }

        public Task<int> UpdateAsync(T entity)
        {
            throw new System.NotImplementedException();
        }
    }
}
