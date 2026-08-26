using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using InventoryAPI.Helpers;
using InventoryAPI.Models;
using InventoryAPI.Repositories;
using InventoryAPI.Services;
using Microsoft.Extensions.Configuration;
using Moq;
using Xunit;

namespace InventoryAPI.Tests
{
    public class ProductServiceTests
    {
        [Fact]
        public async Task CreateProductAsync_WithValidRequest_ReturnsAffectedRows()
        {
            var repoMock = new Mock<IProductRepo>();
            repoMock.Setup(r => r.AddProductAsync(It.IsAny<viewProduct>())).ReturnsAsync(1);
            var service = new ProductService(repoMock.Object);
            var request = new CreateProductRequest
            {
                ProductName = "Test",
                Description = "Desc",
                Price = 10m,
                StockQuantity = 5,
                CategoryID = 1,
                SupplierID = 1
            };

            var result = await service.CreateProductAsync(request);

            Assert.Equal(1, result);
            repoMock.Verify(r => r.AddProductAsync(It.IsAny<viewProduct>()), Times.Once);
        }

        [Fact]
        public async Task CreateProductAsync_NullRequest_ThrowsArgumentNullException()
        {
            var repoMock = new Mock<IProductRepo>();
            var service = new ProductService(repoMock.Object);

            await Assert.ThrowsAsync<ArgumentNullException>(() => service.CreateProductAsync(null));
        }

        [Fact]
        public async Task RemoveProductAsync_CallsRepository_ReturnsTrue()
        {
            var repoMock = new Mock<IProductRepo>();
            repoMock.Setup(r => r.DeleteProductAsync(5)).ReturnsAsync(true);
            var service = new ProductService(repoMock.Object);

            var result = await service.RemoveProductAsync(5);

            Assert.True(result);
            repoMock.Verify(r => r.DeleteProductAsync(5), Times.Once);
        }
    }

    public class SupplierServiceTests
    {
        [Fact]
        public async Task AddSupplier_ReturnsInt()
        {
            var repoMock = new Mock<ISupplierRepository>();
            var supplier = new Supplier { SupplierID = 1, SupplierName = "Acme" };
            repoMock.Setup(r => r.AddSupplier(supplier)).ReturnsAsync(1);
            var service = new SupplierService(repoMock.Object);
            var result = await service.AddSupplier(supplier);
            Assert.Equal(1, result);
        }

        [Fact]
        public async Task GetAllSupplier_ReturnsList()
        {
            var repoMock = new Mock<ISupplierRepository>();
            var list = new List<Supplier> { new Supplier { SupplierID = 1, SupplierName = "A" } };
            repoMock.Setup(r => r.GetAllSupplier()).ReturnsAsync(list);
            var service = new SupplierService(repoMock.Object);

            var result = await service.GetAllSupplier();

            Assert.NotNull(result);
            Assert.Single(result);
        }
    }

    public class CategoryServiceTests
    {
        [Fact]
        public async Task CreateAsync_SetsCreatedAtAndCallsRepo()
        {
            var repoMock = new Mock<ICategoryRepository>();
            repoMock.Setup(r => r.CreateAsync(It.IsAny<InventoryAPI.EntityModel.Category>())).ReturnsAsync(1);
            var service = new CategoryService(repoMock.Object);

            var category = new InventoryAPI.EntityModel.Category { CategoryName = "C" };

            var result = await service.CreateAsync(category);

            Assert.Equal(1, result);
            Assert.True(category.CreatedAt != default);
            repoMock.Verify(r => r.CreateAsync(It.IsAny<InventoryAPI.EntityModel.Category>()), Times.Once);
        }
    }

    public class UserServiceTests
    {
        [Fact]
        public void Authenticate_ValidUser_ReturnsToken()
        {
            var userRepoMock = new Mock<IUserRepo>();
            var user = new User { Username = "test", Role = "Admin" };
            userRepoMock.Setup(r => r.GetUser("u", "p")).Returns(user);

            var inMemorySettings = new Dictionary<string, string?> {
                { "Jwt:Key", "test_secret_which_is_long_enough" },
                { "Jwt:Issuer", "test" },
                { "Jwt:Audience", "test" },
                { "Jwt:DurationInMinutes", "60" }
            };
            IConfiguration configuration = new ConfigurationBuilder().AddInMemoryCollection(inMemorySettings).Build();
            var jwtHelper = new JwtHelper(configuration);

            var service = new UserService(userRepoMock.Object, jwtHelper);

            var response = service.Authenticate(new LoginRequest { Username = "u", Password = "p" });

            Assert.NotNull(response);
            Assert.False(string.IsNullOrWhiteSpace(response.Token));
        }

        [Fact]
        public void Authenticate_InvalidUser_ReturnsNull()
        {
            var userRepoMock = new Mock<IUserRepo>();
            userRepoMock.Setup(r => r.GetUser(It.IsAny<string>(), It.IsAny<string>())).Returns((User)null!);
            IConfiguration configuration = new ConfigurationBuilder().AddInMemoryCollection(new Dictionary<string, string?> {
                { "Jwt:Key", "test_secret_which_is_long_enough" },
                { "Jwt:Issuer", "test" },
                { "Jwt:Audience", "test" },
                { "Jwt:DurationInMinutes", "60" }
            }).Build();
            var jwtHelper = new JwtHelper(configuration);
            var service = new UserService(userRepoMock.Object, jwtHelper);

            var response = service.Authenticate(new LoginRequest { Username = "no", Password = "no" });

            Assert.Null(response);
        }
    }
}
