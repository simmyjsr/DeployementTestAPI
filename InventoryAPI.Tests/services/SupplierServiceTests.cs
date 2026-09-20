using System.Collections.Generic;
using System.Threading.Tasks;
using InventoryAPI.Models;
using InventoryAPI.Repositories;
using InventoryAPI.Services;
using Moq;
using Xunit;

namespace InventoryAPI.Tests.Services
{
    public class SupplierServiceTests
    {
        [Fact]
        public async Task AddSupplier_DelegatesToRepository_ReturnsId()
        {
            var repoMock = new Mock<ISupplierRepository>();
            var supplier = new Supplier { SupplierName = "S" };
            repoMock.Setup(r => r.AddSupplier(supplier)).ReturnsAsync(7);

            var service = new SupplierService(repoMock.Object);

            var id = await service.AddSupplier(supplier);

            Assert.Equal(7, id);
            repoMock.Verify(r => r.AddSupplier(supplier), Times.Once);
        }

        [Fact]
        public async Task GetAllSupplier_ReturnsList()
        {
            var list = new List<Supplier> { new Supplier { SupplierID = 1, SupplierName = "A" } };
            var repoMock = new Mock<ISupplierRepository>();
            repoMock.Setup(r => r.GetAllSupplier()).ReturnsAsync(list);

            var service = new SupplierService(repoMock.Object);

            var result = await service.GetAllSupplier();

            Assert.Equal(list, result);
            repoMock.Verify(r => r.GetAllSupplier(), Times.Once);
        }

        [Fact]
        public async Task GetSupplierById_ReturnsSupplier_WhenFound()
        {
            var expected = new Supplier { SupplierID = 2, SupplierName = "B" };
            var repoMock = new Mock<ISupplierRepository>();
            repoMock.Setup(r => r.GetSupplierById(2)).ReturnsAsync(expected);

            var service = new SupplierService(repoMock.Object);

            var result = await service.GetSupplierById(2);

            Assert.Equal(expected, result);
            repoMock.Verify(r => r.GetSupplierById(2), Times.Once);
        }

        [Fact]
        public async Task GetSupplierById_ReturnsNull_WhenMissing()
        {
            var repoMock = new Mock<ISupplierRepository>();
            repoMock.Setup(r => r.GetSupplierById(99)).ReturnsAsync((Supplier?)null);

            var service = new SupplierService(repoMock.Object);

            var result = await service.GetSupplierById(99);

            Assert.Null(result);
            repoMock.Verify(r => r.GetSupplierById(99), Times.Once);
        }

        [Fact]
        public async Task UpdateSupplier_DelegatesAndReturnsBool()
        {
            var repoMock = new Mock<ISupplierRepository>();
            var supplier = new Supplier { SupplierID = 3, SupplierName = "C" };
            repoMock.Setup(r => r.UpdateSupplier(supplier)).ReturnsAsync(true);

            var service = new SupplierService(repoMock.Object);

            var result = await service.UpdateSupplier(supplier);

            Assert.True(result);
            repoMock.Verify(r => r.UpdateSupplier(supplier), Times.Once);
        }

        [Fact]
        public async Task DeleteSupplier_DelegatesAndReturnsBool()
        {
            var repoMock = new Mock<ISupplierRepository>();
            repoMock.Setup(r => r.DeleteSupplier(5)).ReturnsAsync(false);

            var service = new SupplierService(repoMock.Object);

            var result = await service.DeleteSupplier(5);

            Assert.False(result);
            repoMock.Verify(r => r.DeleteSupplier(5), Times.Once);
        }
    }
}
