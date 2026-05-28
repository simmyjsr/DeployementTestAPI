using Dapper;
using InventoryAPI.Data;
using InventoryAPI.Helpers;
using InventoryAPI.Models;
using InventoryAPI.Repositories;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InventoryAPI.Repositories
{
    public class ProductRepo : IProductRepo
    {
        private readonly DapperContext _context;

        public ProductRepo(DapperContext context)
        {
            _context = context;
        }


        public async Task<bool> DeleteProductAsync(int id)
        {
            using var connection = _context.CreateConnection();
            string query = "DELETE FROM Products WHERE ProductID = @id";
            int rowsAffected = await connection.ExecuteAsync(query, new { id });
            return rowsAffected > 0;
        }

        public async Task<IEnumerable<Product>> GetAllProductsAsync()
        {
            using var connection = _context.CreateConnection();
            var data= await connection.QueryAsync<Product>("SELECT * FROM Products");
            return data;
           // return (IEnumerable<Product>)await connection.QueryAsync<Product>("SELECT * FROM Products");
        }

        public async Task<IEnumerable<Brand>> GetAllBrand()
        {
            using var connection = _context.CreateConnection();
            return (IEnumerable<Brand>)await connection.QueryAsync<Brand>("SELECT BrandID,Name FROM Brand where status =1");
        }
        async Task<Product> IProductRepo.GetProductByIdAsync(int productId)
        {
            using var connection = _context.CreateConnection();
#pragma warning disable CS8603 // Possible null reference return.
            return await connection.QueryFirstOrDefaultAsync<Product>("SELECT * FROM Products WHERE ProductID = @id", new { id = productId });
#pragma warning restore CS8603 // Possible null reference return.
        }

        public async Task<int> AddProductAsync(viewProduct product)
        {
            
            
            const string sql = @"
            INSERT INTO Products 
                (ProductID, ProductName, Description, Price, StockQuantity, CategoryID, ProductNumber, ImageBinary, Feature, CreatedAt, SupplierID) 
            VALUES 
                (@ProductID, @ProductName, @Description, @Price, @StockQuantity, @CategoryID, @ProductNumber, @ImageBytes, @Feature, @CreatedAt, @SupplierID)";

            using (var connection = _context.CreateConnection())
            {
                //await connection.OpenAsync();
                // Dapper maps each @Param to properties on 'product'
                int affectedRows = await connection.ExecuteAsync(sql, product);
               
                return affectedRows;
            }
        }

        public async Task<bool> UpdateProductAsync(viewProduct product)
        {
            using var connection = _context.CreateConnection();
            //string query = "UPDATE Products SET ProductName = @ProductName, Category = @Category, Stock = @Stock, Price = @Price WHERE ProductID = @ProductID";
            int rowsAffected = await connection.ExecuteAsync(Queries.UpdateProducts, product);
            return rowsAffected > 0;
        }


        public async Task<IEnumerable<Product>> SearchProductsAsync(string searchText)
        {
            using var connection = _context.CreateConnection();
            return (IEnumerable<Product>)await connection.QueryAsync<Product>(Queries.GetFilteredProducts, new { searchText = searchText });

        }

    }
}

