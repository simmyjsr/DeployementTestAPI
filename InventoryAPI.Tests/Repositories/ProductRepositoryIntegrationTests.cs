using System.Threading.Tasks;
using Xunit;

namespace InventoryAPI.Tests.Repositories
{
    public class ProductRepositoryIntegrationTests
    {
        [Fact(Skip = "Integration test - requires a configured SQL Server and valid connection string. Enable and configure before running.")]
        public async Task AddProduct_Integration_PlacesRecordInDatabase()
        {
            // This is a placeholder integration test. ProductRepo depends on a real database via DapperContext.
            // To enable: configure a test database, set the connection string in appsettings, and remove Skip attribute.
            await Task.CompletedTask;
        }
    }
}
