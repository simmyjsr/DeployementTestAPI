using Dapper;
using InventoryAPI.Helpers;
using InventoryAPI.Models;
using InventoryAPI.Repositories;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using Microsoft.EntityFrameworkCore;
using InventoryAPI.Data;

namespace InventoryAPI.Repositories
{
    public class ProductRepo : IProductRepo
    {
        private readonly AppDbContext _db;

        public ProductRepo(AppDbContext db)
        {
            _db = db;
        }

        // Delete using EF for clarity
        public async Task<bool> DeleteProductAsync(int id)
        {
            // assuming ProductID maps to an int in DB schema; use Dapper for exact behavior
            var conn = _db.Database.GetDbConnection();
            if (conn.State != ConnectionState.Open) await conn.OpenAsync();
            string query = "DELETE FROM Products WHERE ProductID = @id";
            int rowsAffected = await conn.ExecuteAsync(query, new { id });
            return rowsAffected > 0;
        }

        // Get all products using EF Core
        public async Task<IEnumerable<Product>> GetAllProductsAsync()
        {
            return await _db.Set<Product>().AsNoTracking().ToListAsync();
        }

        // Keep brand query in Dapper as a hot/raw SQL example
        public async Task<IEnumerable<Brand>> GetAllBrand()
        {
            var conn = _db.Database.GetDbConnection();
            if (conn.State != ConnectionState.Open) await conn.OpenAsync();
            return await conn.QueryAsync<Brand>("SELECT BrandID,Name FROM Brand where status =1");
        }

        async Task<Product> IProductRepo.GetProductByIdAsync(int productId)
        {
            // Use EF for retrieval by key if possible
            // Note: interface expects int productId; adapt as needed for your schema
            return await _db.Set<Product>().FindAsync(productId).AsTask();
        }

        public async Task<int> AddProductAsync(viewProduct product)
        {
            // Map viewProduct to Product model and use EF to insert
            var entity = new Product
            {
                ProductID = product.ProductID,
                ProductName = product.ProductName,
                Description = product.Description,
                Price = product.Price,
                StockQuantity = product.StockQuantity,
                CategoryID = product.CategoryID,
                ProductNumber = product.ProductNumber,
                ImageBinary = product.ImageBytes,
                Feature = product.Feature,
                CreatedAt = product.CreatedAt,
                SupplierID = product.SupplierID
            };

            _db.Set<Product>().Add(entity);
            var changed = await _db.SaveChangesAsync();
            return changed;
        }

        public async Task<bool> UpdateProductAsync(viewProduct product)
        {
            // Map viewProduct to Product and update via EF
            var entity = new Product
            {
                ProductID = product.ProductID,
                ProductName = product.ProductName,
                Description = product.Description,
                Price = product.Price,
                StockQuantity = product.StockQuantity,
                CategoryID = product.CategoryID,
                ProductNumber = product.ProductNumber,
                ImageBinary = product.ImageBytes,
                Feature = product.Feature,
                CreatedAt = product.CreatedAt,
                SupplierID = product.SupplierID
            };

            _db.Set<Product>().Update(entity);
            var changed = await _db.SaveChangesAsync();
            return changed > 0;
        }


        public async Task<IEnumerable<Product>> SearchProductsAsync(string searchText)
        {
            var conn = _db.Database.GetDbConnection();
            if (conn.State != ConnectionState.Open) await conn.OpenAsync();
            return await conn.QueryAsync<Product>(Queries.GetFilteredProducts, new { searchText = searchText });

        }

    }
}
