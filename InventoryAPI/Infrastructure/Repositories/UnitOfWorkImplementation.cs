using System.Threading.Tasks;
using System.Data;
using Microsoft.Data.SqlClient;
using System.Data.Common;

namespace InventoryAPI.Infrastructure.Repositories
{
    public class UnitOfWorkImplementation : IUnitOfWork
    {
        private readonly Data.DapperContext _context;
        private IDbConnection? _connection;
        private IDbTransaction? _transaction;

        public UnitOfWorkImplementation(Data.DapperContext context)
        {
            _context = context;
        }

        public IDbConnection Connection => _connection ??= _context.CreateConnection();

        public IDbTransaction? Transaction => _transaction;

        public async Task BeginTransactionAsync()
        {
            if (_connection == null)
                _connection = _context.CreateConnection();

            if (_connection.State != ConnectionState.Open)
                await ((DbConnection)_connection).OpenAsync();

            _transaction = _connection.BeginTransaction();
        }

        public Task CommitAsync()
        {
            if (_transaction == null) throw new InvalidOperationException("No active transaction.");
            _transaction.Commit();
            _transaction.Dispose();
            _transaction = null;
            return Task.CompletedTask;
        }

        public Task RollbackAsync()
        {
            if (_transaction == null) throw new InvalidOperationException("No active transaction.");
            _transaction.Rollback();
            _transaction.Dispose();
            _transaction = null;
            return Task.CompletedTask;
        }

        public Task<int> SaveChangesAsync()
        {
            // For Dapper, changes are executed immediately by SQL commands. Return 0 as placeholder.
            return Task.FromResult(0);
        }

        public void Dispose()
        {
            _transaction?.Dispose();
            if (_connection != null)
            {
                _connection.Dispose();
                _connection = null;
            }
        }
    }
}
