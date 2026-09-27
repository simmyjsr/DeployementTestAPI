using System.Text;
using InventoryAPI.Models;
using InventoryAPI.Repositories;
using Microsoft.AspNetCore.Http;
using Moq;

namespace InventoryAPI.Tests.Services
{
    public class ProductServiceTests
    {
        [Fact]
        public async Task GetAllProductsAsync_DelegatesToRepository()
        {
            var expected = new List<Product> { new Product { ProductID = Guid.NewGuid(), ProductName = "P" } };
            var repoMock = new Mock<IProductRepo>();
            repoMock.Setup(r => r.GetAllProductsAsync()).ReturnsAsync(expected);

            var service = new ProductService(repoMock.Object);

            var result = await service.GetAllProductsAsync();

            Assert.Equal(expected, result);
            repoMock.Verify(r => r.GetAllProductsAsync(), Times.Once);
        }

        [Fact]
        public async Task GetAllBrand_DelegatesToRepository()
        {
            var expected = new List<Brand> { new Brand { BrandID = 1, Name = "B" } };
            var repoMock = new Mock<IProductRepo>();
            repoMock.Setup(r => r.GetAllBrand()).ReturnsAsync(expected);

            var service = new ProductService(repoMock.Object);

            var result = await service.GetAllBrand();

            Assert.Equal(expected, result);
            repoMock.Verify(r => r.GetAllBrand(), Times.Once);
        }

        [Fact]
        public async Task GetProductByIdAsync_DelegatesToRepository()
        {
            var expected = new Product { ProductID = Guid.NewGuid(), ProductName = "X" };
            var repoMock = new Mock<IProductRepo>();
            repoMock.Setup(r => r.GetProductByIdAsync(5)).ReturnsAsync(expected);

            var service = new ProductService(repoMock.Object);

            var result = await service.GetProductByIdAsync(5);

            Assert.Equal(expected, result);
            repoMock.Verify(r => r.GetProductByIdAsync(5), Times.Once);
        }

        [Fact]
        public async Task CreateProductAsync_NullRequest_Throws()
        {
            var repoMock = new Mock<IProductRepo>();
            var service = new ProductService(repoMock.Object);

            await Assert.ThrowsAsync<ArgumentNullException>(() => service.CreateProductAsync(null!));
        }

        [Fact]
        public async Task CreateProductAsync_WithImage_CopiesBytesAndCallsRepository()
        {
            var repoMock = new Mock<IProductRepo>();
            viewProduct captured = null!;
            repoMock.Setup(r => r.AddProductAsync(It.IsAny<viewProduct>())).ReturnsAsync(1)
                    .Callback<viewProduct>(p => captured = p);

            var service = new ProductService(repoMock.Object);

            var imageBytes = Encoding.UTF8.GetBytes("dummy-image");
            var stream = new MemoryStream(imageBytes);
            var formFile = new FormFile(stream, 0, stream.Length, "image", "img.jpg");

            var request = new CreateProductRequest
            {
                ProductName = "New",
                Description = "D",
                Price = 1.2m,
                StockQuantity = 3,
                CategoryID = 2,
                SupplierID = 4,
                Image = formFile
            };

            var result = await service.CreateProductAsync(request);

            Assert.Equal(1, result);
            Assert.NotNull(captured);
            Assert.NotNull(captured.ImageBytes);
            Assert.Equal(imageBytes.Length, captured.ImageBytes.Length);
            Assert.Equal("seema", captured.CreatedBy);
            Assert.StartsWith("PRD-", captured.ProductNumber);
            repoMock.Verify(r => r.AddProductAsync(It.IsAny<viewProduct>()), Times.Once);
        }

        [Fact]
        public async Task CreateProductAsync_WithoutImage_SetsNullImageBytes()
        {
            var repoMock = new Mock<IProductRepo>();
            viewProduct captured = null!;
            repoMock.Setup(r => r.AddProductAsync(It.IsAny<viewProduct>())).ReturnsAsync(1)
                    .Callback<viewProduct>(p => captured = p);

            var service = new ProductService(repoMock.Object);

            var request = new CreateProductRequest
            {
                ProductName = "NoImg",
                Description = "D",
                Price = 2.2m,
                StockQuantity = 1,
                CategoryID = 2,
                SupplierID = 3,
                Image = null
            };

            var result = await service.CreateProductAsync(request);

            Assert.Equal(1, result);
            Assert.NotNull(captured);
            Assert.Null(captured.ImageBytes);
            Assert.Equal("seema", captured.CreatedBy);
            Assert.StartsWith("PRD-", captured.ProductNumber);
        }

        [Fact]
        public async Task UpdateProductAsync_SetsUpdatedFields_AndCallsRepo()
        {
            var repoMock = new Mock<IProductRepo>();
            viewProduct captured = null!;
            repoMock.Setup(r => r.UpdateProductAsync(It.IsAny<viewProduct>())).ReturnsAsync(true)
                    .Callback<viewProduct>(p => captured = p);

            var service = new ProductService(repoMock.Object);
            var product = new viewProduct { ProductID = Guid.NewGuid(), ProductName = "U" };

            var result = await service.UpdateProductAsync(product);

            Assert.True(result);
            Assert.NotNull(captured);
            Assert.Equal("seema", captured.UpdatedBy);
            Assert.True(captured.UpdatedAt != default);
            repoMock.Verify(r => r.UpdateProductAsync(It.IsAny<viewProduct>()), Times.Once);
        }

        [Fact]
        public async Task RemoveProductAsync_DelegatesToRepository()
        {
            var repoMock = new Mock<IProductRepo>();
            repoMock.Setup(r => r.DeleteProductAsync(9)).ReturnsAsync(true);

            var service = new ProductService(repoMock.Object);

            var result = await service.RemoveProductAsync(9);

            Assert.True(result);
            repoMock.Verify(r => r.DeleteProductAsync(9), Times.Once);
        }

        [Fact]
        public async Task SearchProductsAsync_DelegatesToRepository()
        {
            var expected = new List<Product> { new Product { ProductID = Guid.NewGuid(), ProductName = "S" } };
            var repoMock = new Mock<IProductRepo>();
            repoMock.Setup(r => r.SearchProductsAsync("term")).ReturnsAsync(expected);

            var service = new ProductService(repoMock.Object);

            var result = await service.SearchProductsAsync("term");

            Assert.Equal(expected, result);
            repoMock.Verify(r => r.SearchProductsAsync("term"), Times.Once);
        }
    }
}
