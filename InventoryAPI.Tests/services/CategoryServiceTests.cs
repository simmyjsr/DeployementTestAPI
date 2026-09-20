using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using InventoryAPI.EntityModel;
using InventoryAPI.Repositories;
using InventoryAPI.Services;
using Moq;
using Xunit;

namespace InventoryAPI.Tests.Services
{
    public class CategoryServiceTests
    {
        [Fact]
        public async Task GetAllAsync_DelegatesToRepository()
        {
            var expected = new List<Category> { new Category { CategoryID = 1, CategoryName = "C" } };
            var repoMock = new Mock<ICategoryRepository>();
            repoMock.Setup(r => r.GetAllAsync()).ReturnsAsync(expected);

            var service = new CategoryService(repoMock.Object);

            var result = await service.GetAllAsync();

            Assert.Equal(expected, result);
            repoMock.Verify(r => r.GetAllAsync(), Times.Once);
        }

        [Fact]
        public async Task GetByIdAsync_DelegatesToRepository()
        {
            var expected = new Category { CategoryID = 2, CategoryName = "X" };
            var repoMock = new Mock<ICategoryRepository>();
            repoMock.Setup(r => r.GetByIdAsync(2)).ReturnsAsync(expected);

            var service = new CategoryService(repoMock.Object);

            var result = await service.GetByIdAsync(2);

            Assert.Equal(expected, result);
            repoMock.Verify(r => r.GetByIdAsync(2), Times.Once);
        }

        [Fact]
        public async Task CreateAsync_SetsCreatedAt_AndCallsRepository()
        {
            var repoMock = new Mock<ICategoryRepository>();
            repoMock.Setup(r => r.CreateAsync(It.IsAny<Category>())).ReturnsAsync(42);

            var service = new CategoryService(repoMock.Object);

            var category = new Category { CategoryName = "New" };

            var before = DateTime.UtcNow.AddSeconds(-5);
            var id = await service.CreateAsync(category);
            var after = DateTime.UtcNow.AddSeconds(5);

            Assert.Equal(42, id);
            Assert.True(category.CreatedAt != default, "CreatedAt should be set");
            Assert.InRange(category.CreatedAt, before, after);
            repoMock.Verify(r => r.CreateAsync(It.Is<Category>(c => c == category)), Times.Once);
        }

        [Fact]
        public async Task UpdateAsync_DelegatesToRepository()
        {
            var repoMock = new Mock<ICategoryRepository>();
            repoMock.Setup(r => r.UpdateAsync(It.IsAny<Category>())).ReturnsAsync(true);

            var service = new CategoryService(repoMock.Object);

            var category = new Category { CategoryID = 5, CategoryName = "U" };

            var result = await service.UpdateAsync(category);

            Assert.True(result);
            repoMock.Verify(r => r.UpdateAsync(category), Times.Once);
        }

        [Fact]
        public async Task DeleteAsync_DelegatesToRepository()
        {
            var repoMock = new Mock<ICategoryRepository>();
            repoMock.Setup(r => r.DeleteAsync(7)).ReturnsAsync(false);

            var service = new CategoryService(repoMock.Object);

            var result = await service.DeleteAsync(7);

            Assert.False(result);
            repoMock.Verify(r => r.DeleteAsync(7), Times.Once);
        }
    }
}
