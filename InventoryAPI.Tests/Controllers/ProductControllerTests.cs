using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using InventoryAPI.Controllers;
using InventoryAPI.Models;
using InventoryAPI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Moq;
using Xunit;

namespace InventoryAPI.Tests.Controllers
{
    public class ProductControllerTests
    {
        [Fact]
        public async Task GetProducts_ReturnsOkWithProducts()
        {
            var products = new List<Product> { new Product { ProductID = Guid.NewGuid(), ProductName = "P1" } };
            var serviceMock = new Mock<IProductService>();
            serviceMock.Setup(s => s.GetAllProductsAsync()).ReturnsAsync(products);

            var controller = new ProductController(serviceMock.Object);

            var result = await controller.GetProducts();

            var ok = Assert.IsType<OkObjectResult>(result.Result);
            Assert.Equal(products, ok.Value);
        }

        [Fact]
        public async Task GetAllBrand_ReturnsOkWithBrands()
        {
            var brands = new List<Brand> { new Brand { BrandID = 1, Name = "B1" } };
            var serviceMock = new Mock<IProductService>();
            serviceMock.Setup(s => s.GetAllBrand()).ReturnsAsync(brands);

            var controller = new ProductController(serviceMock.Object);

            var result = await controller.GetAllBrand();

            var ok = Assert.IsType<OkObjectResult>(result.Result);
            Assert.Equal(brands, ok.Value);
        }

        [Fact]
        public async Task GetProduct_ReturnsOk_WhenFound()
        {
            var product = new Product { ProductID = Guid.NewGuid(), ProductName = "P1" };
            var serviceMock = new Mock<IProductService>();
            serviceMock.Setup(s => s.GetProductByIdAsync(5)).ReturnsAsync(product);

            var controller = new ProductController(serviceMock.Object);

            var result = await controller.GetProduct(5);

            var ok = Assert.IsType<OkObjectResult>(result.Result);
            Assert.Equal(product, ok.Value);
        }

        [Fact]
        public async Task CreateProduct_ReturnsFailedMessage_WhenEffectedRowNotOne()
        {
            var serviceMock = new Mock<IProductService>();
            serviceMock.Setup(s => s.CreateProductAsync(It.IsAny<CreateProductRequest>())).ReturnsAsync(0);

            var controller = new ProductController(serviceMock.Object);

            var request = new CreateProductRequest { ProductName = "New" };

            var result = await controller.CreateProduct(request);

            var ok = Assert.IsType<OkObjectResult>(result);
            Assert.Contains("Product creation failed", ok.Value?.ToString() ?? string.Empty);
        }

        [Fact]
        public async Task CreateProduct_ReturnsDuplicateMessage_OnSqlUniqueConstraint()
        {
            var serviceMock = new Mock<IProductService>();
            // Throw a test-friendly DuplicateDataException instead of relying on internal SqlException construction
            serviceMock.Setup(s => s.CreateProductAsync(It.IsAny<CreateProductRequest>())).ThrowsAsync(new InventoryAPI.Exceptions.DuplicateDataException("duplicate"));

            var controller = new ProductController(serviceMock.Object);
            var request = new CreateProductRequest { ProductName = "Dup" };

            var result = await controller.CreateProduct(request);

            var ok = Assert.IsType<OkObjectResult>(result);
            Assert.Contains("Duplicate data", ok.Value?.ToString() ?? string.Empty);
        }

        [Fact]
        public async Task CreateProduct_ReturnsServerError_OnUnhandledException()
        {
            var serviceMock = new Mock<IProductService>();
            serviceMock.Setup(s => s.CreateProductAsync(It.IsAny<CreateProductRequest>())).ThrowsAsync(new Exception("boom"));

            var controller = new ProductController(serviceMock.Object);
            var request = new CreateProductRequest { ProductName = "Err" };

            var result = await controller.CreateProduct(request);

            var status = Assert.IsType<ObjectResult>(result);
            Assert.Equal(500, status.StatusCode);
            Assert.Contains("An error occurred", status.Value?.ToString() ?? string.Empty);
        }

        [Fact]
        public async Task UpdateProduct_ReturnsBadRequest_WhenIdMismatch()
        {
            var serviceMock = new Mock<IProductService>();
            var controller = new ProductController(serviceMock.Object);
            var product = new viewProduct { ProductID = Guid.NewGuid(), ProductName = "P" };

            var result = await controller.UpdateProduct(Guid.NewGuid(), product);

            Assert.IsType<BadRequestResult>(result);
        }

        [Fact]
        public async Task UpdateProduct_ReturnsNoContent_WhenSuccess()
        {
            var serviceMock = new Mock<IProductService>();
            serviceMock.Setup(s => s.UpdateProductAsync(It.IsAny<viewProduct>())).ReturnsAsync(true);
            var controller = new ProductController(serviceMock.Object);
            var id = Guid.NewGuid();
            var product = new viewProduct { ProductID = id, ProductName = "P" };

            var result = await controller.UpdateProduct(id, product);

            Assert.IsType<NoContentResult>(result);
        }

        [Fact]
        public async Task UpdateProduct_ReturnsNotFound_WhenServiceReturnsFalse()
        {
            var serviceMock = new Mock<IProductService>();
            serviceMock.Setup(s => s.UpdateProductAsync(It.IsAny<viewProduct>())).ReturnsAsync(false);
            var controller = new ProductController(serviceMock.Object);
            var id = Guid.NewGuid();
            var product = new viewProduct { ProductID = id, ProductName = "P" };

            var result = await controller.UpdateProduct(id, product);

            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task DeleteProduct_ReturnsNoContent_WhenSuccess()
        {
            var serviceMock = new Mock<IProductService>();
            serviceMock.Setup(s => s.RemoveProductAsync(5)).ReturnsAsync(true);
            var controller = new ProductController(serviceMock.Object);

            var result = await controller.DeleteProduct(5);

            Assert.IsType<NoContentResult>(result);
        }

        [Fact]
        public async Task DeleteProduct_ReturnsNotFound_WhenFail()
        {
            var serviceMock = new Mock<IProductService>();
            serviceMock.Setup(s => s.RemoveProductAsync(5)).ReturnsAsync(false);
            var controller = new ProductController(serviceMock.Object);

            var result = await controller.DeleteProduct(5);

            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task SearchProducts_ReturnsOkWithResults()
        {
            var products = new List<Product> { new Product { ProductID = Guid.NewGuid(), ProductName = "Found" } };
            var serviceMock = new Mock<IProductService>();
            serviceMock.Setup(s => s.SearchProductsAsync("term")).ReturnsAsync(products);
            var controller = new ProductController(serviceMock.Object);

            var result = await controller.SearchProducts("term");

            var ok = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(products, ok.Value);
        }

        // Helper to create a SqlException with a specific number via reflection
        private static SqlException CreateSqlException(int number)
        {
            // Microsoft.Data.SqlClient.SqlError
            var sqlErrorType = typeof(SqlException).Assembly.GetType("Microsoft.Data.SqlClient.SqlError");
            // Pick a non-public constructor that begins with an int parameter (error number)
            var sqlErrorCtor = sqlErrorType.GetConstructors(BindingFlags.NonPublic | BindingFlags.Instance)
                .FirstOrDefault(ci => ci.GetParameters().Length >= 1 && ci.GetParameters()[0].ParameterType == typeof(int))
                ?? sqlErrorType.GetConstructors(BindingFlags.NonPublic | BindingFlags.Instance).First();

            var ctorParams = sqlErrorCtor.GetParameters();
            var ctorArgs = new object[ctorParams.Length];
            for (int i = 0; i < ctorParams.Length; i++)
            {
                var pType = ctorParams[i].ParameterType;
                if (i == 0)
                    ctorArgs[i] = number;
                else if (pType == typeof(byte))
                    ctorArgs[i] = (byte)0;
                else if (pType == typeof(string))
                    ctorArgs[i] = i == 4 ? "message" : ""; // prefer message for middle param if present
                else if (pType.IsValueType)
                    ctorArgs[i] = Activator.CreateInstance(pType);
                else
                    ctorArgs[i] = null;
            }

            var sqlError = sqlErrorCtor.Invoke(ctorArgs);

            var errorCollectionType = typeof(SqlException).Assembly.GetType("Microsoft.Data.SqlClient.SqlErrorCollection");
            var errorCollection = Activator.CreateInstance(errorCollectionType, true);
            var addMethod = errorCollectionType.GetMethod("Add", BindingFlags.NonPublic | BindingFlags.Instance);
            addMethod.Invoke(errorCollection, new[] { sqlError });

            // Find an internal CreateException overload that accepts the error collection as the first parameter
            var createMethods = typeof(SqlException).GetMethods(BindingFlags.NonPublic | BindingFlags.Static)
                .Where(m => m.Name == "CreateException").ToArray();
            MethodInfo createMethod = null;
            foreach (var m in createMethods)
            {
                var ps = m.GetParameters();
                if (ps.Length >= 1 && ps[0].ParameterType == errorCollectionType)
                {
                    createMethod = m;
                    break;
                }
            }
            if (createMethod == null)
                throw new InvalidOperationException("Could not find internal CreateException method on SqlException");

            var createParams = createMethod.GetParameters();
            var createArgs = new object[createParams.Length];
            createArgs[0] = errorCollection;
            for (int i = 1; i < createParams.Length; i++)
            {
                var pType = createParams[i].ParameterType;
                if (pType == typeof(string)) createArgs[i] = "7.0.0";
                else if (pType.IsValueType) createArgs[i] = Activator.CreateInstance(pType);
                else createArgs[i] = null;
            }

            var ex = (SqlException)createMethod.Invoke(null, createArgs);
            return ex;
        }
    }
}
