
using Dapper;
using InventoryAPI.Data;
using InventoryAPI.Models;
using Microsoft.Data.SqlClient;
using System.Collections.Generic;

namespace InventoryAPI.Repositories
{
    public class SupplierRepository : ISupplierRepository
    {
        private readonly DapperContext _context;

        public SupplierRepository(DapperContext context)
        {
            _context = context;
        }
        public async Task<int> AddSupplier(Supplier supplier)
        {
            using var connection = _context.CreateConnection();
            supplier.CreatedAt = DateTime.UtcNow;
            var sql = @"INSERT INTO Suppliers (SupplierName, ContactPerson, Email, Phone, Address, CreatedAt, Status) 
                    VALUES (@SupplierName, @ContactPerson, @Email, @Phone, @Address, @CreatedAt, @Status)";
           int affectedRows= await connection.ExecuteAsync(sql, supplier);
            return affectedRows;
        }

        public async Task<bool> DeleteSupplier(int id)
        {
            using var connection = _context.CreateConnection();
            int rowsAffected = await connection.ExecuteAsync("DELETE FROM Suppliers WHERE SupplierID = @Id", new { Id = id });
            return rowsAffected > 0;
        }

        public async Task<IEnumerable<Supplier>> GetAllSupplier()
        {
            using var connection = _context.CreateConnection();
              var data=await connection.QueryAsync<Supplier>("SELECT SupplierID,SupplierName FROM SUPPLIERS");
            return data;
            //return (IEnumerable<Product>)await connection.QueryAsync<Product>("SELECT * FROM Products");

        }

        public async Task<bool> UpdateSupplier(Supplier supplier)
        {
            using var connection = _context.CreateConnection();
            var sql = @"UPDATE Suppliers 
                    SET SupplierName = @SupplierName, ContactPerson = @ContactPerson, Email = @Email, 
                        Phone = @Phone, Address = @Address, Status = @Status 
                    WHERE SupplierID = @SupplierID";
           int rowsAffected = await connection.ExecuteAsync(sql, supplier);
            return rowsAffected > 0;
        }

        public async Task<Supplier?> GetSupplierById(int id)
        {
            using var connection = _context.CreateConnection();
            return await connection.QueryFirstOrDefaultAsync<Supplier>(
                "SELECT SupplierID,SupplierName FROM Suppliers WHERE SupplierID = @Id", new { Id = id });
        }

       
    }
}
