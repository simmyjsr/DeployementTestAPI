using System.Collections.Generic;
using System.Threading.Tasks;
using InventoryAPI.Controllers;
using InventoryAPI.EntityModel;
using InventoryAPI.Services;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace InventoryAPI.Tests.Controllers
{
    public class CategoryControllerTests
    {
        [Fact]
        public async Task GetAll_ReturnsOkWithCategories()
        {
            var categories = new List<Category> { new Category { CategoryID = 1, CategoryName = "C1" } };
            var serviceMock = new Mock<ICategoryService>();
            serviceMock.Setup(s => s.GetAllAsync()).ReturnsAsync(categories);

            var controller = new CategoryController(serviceMock.Object);

            var result = await controller.GetAll();

            var ok = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(categories, ok.Value);
        }

        [Fact]
        public async Task Get_ReturnsNotFound_WhenMissing()
        {
            var serviceMock = new Mock<ICategoryService>();
            serviceMock.Setup(s => s.GetByIdAsync(5)).ReturnsAsync((Category?)null);

            var controller = new CategoryController(serviceMock.Object);

            var result = await controller.Get(5);

            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task Get_ReturnsOk_WhenFound()
        {
            var expected = new Category { CategoryID = 2, CategoryName = "C2" };
            var serviceMock = new Mock<ICategoryService>();
            serviceMock.Setup(s => s.GetByIdAsync(2)).ReturnsAsync(expected);

            var controller = new CategoryController(serviceMock.Object);

            var result = await controller.Get(2);

            var ok = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(expected, ok.Value);
        }

        [Fact]
        public async Task Create_ReturnsCreatedAtAction_WithCategory()
        {
            var serviceMock = new Mock<ICategoryService>();
            serviceMock.Setup(s => s.CreateAsync(It.IsAny<Category>())).ReturnsAsync(10);

            var controller = new CategoryController(serviceMock.Object);
            var category = new Category { CategoryName = "New" };

            var result = await controller.Create(category);

            var created = Assert.IsType<CreatedAtActionResult>(result);
            Assert.Equal("Get", created.ActionName);
            Assert.Equal(10, ((Category)created.Value).CategoryID);
            Assert.Equal(category.CategoryName, ((Category)created.Value).CategoryName);
        }

        [Fact]
        public async Task Update_ReturnsBadRequest_OnIdMismatch()
        {
            var serviceMock = new Mock<ICategoryService>();
            var controller = new CategoryController(serviceMock.Object);
            var category = new Category { CategoryID = 3, CategoryName = "X" };

            var result = await controller.Update(4, category);

            Assert.IsType<BadRequestResult>(result);
        }

        [Fact]
        public async Task Update_ReturnsOk_WhenSuccess()
        {
            var serviceMock = new Mock<ICategoryService>();
            serviceMock.Setup(s => s.UpdateAsync(It.IsAny<Category>())).ReturnsAsync(true);
            var controller = new CategoryController(serviceMock.Object);
            var category = new Category { CategoryID = 5, CategoryName = "Y" };

            var result = await controller.Update(5, category);

            Assert.IsType<OkResult>(result);
        }

        [Fact]
        public async Task Update_ReturnsNotFound_WhenServiceReturnsFalse()
        {
            var serviceMock = new Mock<ICategoryService>();
            serviceMock.Setup(s => s.UpdateAsync(It.IsAny<Category>())).ReturnsAsync(false);
            var controller = new CategoryController(serviceMock.Object);
            var category = new Category { CategoryID = 6, CategoryName = "Z" };

            var result = await controller.Update(6, category);

            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task Delete_ReturnsNoContent_WhenSuccess()
        {
            var serviceMock = new Mock<ICategoryService>();
            serviceMock.Setup(s => s.DeleteAsync(7)).ReturnsAsync(true);
            var controller = new CategoryController(serviceMock.Object);

            var result = await controller.Delete(7);

            Assert.IsType<NoContentResult>(result);
        }

        [Fact]
        public async Task Delete_ReturnsNotFound_WhenFail()
        {
            var serviceMock = new Mock<ICategoryService>();
            serviceMock.Setup(s => s.DeleteAsync(8)).ReturnsAsync(false);
            var controller = new CategoryController(serviceMock.Object);

            var result = await controller.Delete(8);

            Assert.IsType<NotFoundResult>(result);
        }
    }
}
