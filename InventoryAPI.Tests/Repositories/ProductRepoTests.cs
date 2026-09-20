using System;
using System.Threading.Tasks;
using Xunit;

namespace InventoryAPI.Tests.Repositories
{
    public class ProductRepoTests
    {
        [Fact(Skip = "Integration test - requires a SQL Server or SQLite test database and test DapperContext. Enable and configure before running.")]
        public async Task AddProductAsync_InsertsRecord()
        {
            // Arrange
            // 1. Create a test database (LocalDB or SQLite in-memory)
            // 2. Create Products table matching schema used by ProductRepo
            // 3. Create a DapperContext pointing to that test database
            // 4. Instantiate ProductRepo with the DapperContext

            // Act
            // Call AddProductAsync with a populated viewProduct

            // Assert
            // Verify returned affected rows == 1 and the record exists in DB

            await Task.CompletedTask;
        }

        [Fact(Skip = "Integration test - requires a SQL Server or SQLite test database and test DapperContext. Enable and configure before running.")]
        public async Task GetAllProductsAsync_ReturnsRecords()
        {
            // Arrange - seed Products table with known rows

            // Act - call GetAllProductsAsync

            // Assert - returned collection contains seeded rows

            await Task.CompletedTask;
        }

        [Fact(Skip = "Integration test - requires a SQL Server or SQLite test database and test DapperContext. Enable and configure before running.")]
        public async Task GetProductByIdAsync_ReturnsSingleOrNull()
        {
            // Arrange - insert one product

            // Act - call GetProductByIdAsync with existing and non-existing id

            // Assert - existing id returns product, missing id returns null

            await Task.CompletedTask;
        }

        [Fact(Skip = "Integration test - requires a SQL Server or SQLite test database and test DapperContext. Enable and configure before running.")]
        public async Task UpdateProductAsync_UpdatesRecord()
        {
            // Arrange - insert one product

            // Act - modify product and call UpdateProductAsync

            // Assert - method returns true and DB reflects update

            await Task.CompletedTask;
        }

        [Fact(Skip = "Integration test - requires a SQL Server or SQLite test database and test DapperContext. Enable and configure before running.")]
        public async Task DeleteProductAsync_RemovesRecord()
        {
            // Arrange - insert one product

            // Act - call DeleteProductAsync

            // Assert - method returns true and DB no longer contains product

            await Task.CompletedTask;
        }

        [Fact(Skip = "Integration test - requires a SQL Server or SQLite test database and test DapperContext. Enable and configure before running.")]
        public async Task SearchProductsAsync_ReturnsFilteredResults()
        {
            // Arrange - create products and categories for search

            // Act - call SearchProductsAsync with a search term

            // Assert - results contain matched product(s)

            await Task.CompletedTask;
        }
    }
}
