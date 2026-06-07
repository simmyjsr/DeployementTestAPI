
using Dapper;
using InventoryAPI.Data;
using InventoryAPI.Infrastructure;
using InventoryAPI.Models;
using Microsoft.Data.SqlClient;
using System.Collections.Generic;

namespace InventoryAPI.Repositories
{
    public class SupplierRepository : ISupplierRepository
    {
        private readonly DapperContext _context;

        // SQL loaded from embedded resources
        private string SqlInsert => SqlResourceLoader.GetSql("Suppliers.Insert.sql");
        private string SqlDelete => SqlResourceLoader.GetSql("Suppliers.Delete.sql");
        private string SqlGetAll => SqlResourceLoader.GetSql("Suppliers.GetAll.sql");
        private string SqlGetById => SqlResourceLoader.GetSql("Suppliers.GetById.sql");
        private string SqlUpdate => SqlResourceLoader.GetSql("Suppliers.Update.sql");

        public SupplierRepository(DapperContext context)
        {
            _context = context;
        }
        public async Task<int> AddSupplier(Supplier supplier)
        {
            using var connection = _context.CreateConnection();
            supplier.CreatedAt = DateTime.UtcNow;
           // return the inserted identity id
           var id = await connection.ExecuteScalarAsync<int>(SqlInsert, supplier);
           return id;
        }

        public async Task<bool> DeleteSupplier(int id)
        {
            using var connection = _context.CreateConnection();
            int rowsAffected = await connection.ExecuteAsync(SqlDelete, new { Id = id });
            return rowsAffected > 0;
        }

        public async Task<IEnumerable<Supplier>> GetAllSupplier()
        {
            using var connection = _context.CreateConnection();
              var data = await connection.QueryAsync<Supplier>(SqlGetAll);
            return data;
        }

        public async Task<bool> UpdateSupplier(Supplier supplier)
        {
            using var connection = _context.CreateConnection();
           int rowsAffected = await connection.ExecuteAsync(SqlUpdate, supplier);
            return rowsAffected > 0;
        }

        public async Task<Supplier?> GetSupplierById(int id)
        {
            using var connection = _context.CreateConnection();
            return await connection.QueryFirstOrDefaultAsync<Supplier>(SqlGetById, new { Id = id });
        }

       
    }
}
