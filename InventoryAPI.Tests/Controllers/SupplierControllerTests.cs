using System.Collections.Generic;
using InventoryAPI.Controllers;
using InventoryAPI.Models;
using InventoryAPI.Services;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace InventoryAPI.Tests.Controllers
{
    public class SupplierControllerTests
    {
        [Fact]
        public async System.Threading.Tasks.Task GetAll_ReturnsOkWithSuppliers()
        {
            var suppliers = new List<Supplier> { new Supplier { SupplierID = 1, SupplierName = "S1" } };
            var serviceMock = new Mock<ISupplierService>();
            serviceMock.Setup(s => s.GetAllSupplier()).ReturnsAsync(suppliers);

            var controller = new SupplierController(serviceMock.Object);

            var action = await controller.GetAll();

            var ok = Assert.IsType<OkObjectResult>(action.Result);
            Assert.Equal(suppliers, ok.Value);
        }

        [Fact]
        public async System.Threading.Tasks.Task GetById_ReturnsOkWithNull_WhenMissing()
        {
            var serviceMock = new Mock<ISupplierService>();
            serviceMock.Setup(s => s.GetSupplierById(1)).ReturnsAsync((Supplier?)null);

            var controller = new SupplierController(serviceMock.Object);

            var result = controller.GetById(1);

            var ok = Assert.IsType<OkObjectResult>(result);
            // Controller currently returns the Task<Supplier> directly; unwrap and ensure the task's result is null
            var task = Assert.IsAssignableFrom<System.Threading.Tasks.Task<Supplier>>(ok.Value);
            var supplier = await task;
            Assert.Null(supplier);
        }

        [Fact]
        public async System.Threading.Tasks.Task GetById_ReturnsOkWithSupplier_WhenFound()
        {
            var expected = new Supplier { SupplierID = 2, SupplierName = "S2" };
            var serviceMock = new Mock<ISupplierService>();
            serviceMock.Setup(s => s.GetSupplierById(2)).ReturnsAsync(expected);

            var controller = new SupplierController(serviceMock.Object);

            var result = controller.GetById(2);

            var ok = Assert.IsType<OkObjectResult>(result);
            var task = Assert.IsAssignableFrom<System.Threading.Tasks.Task<Supplier>>(ok.Value);
            var supplier = await task;
            Assert.Equal(expected, supplier);
        }

        [Fact]
        public void Create_CallsService_AndReturnsOk()
        {
            var serviceMock = new Mock<ISupplierService>();
            var controller = new SupplierController(serviceMock.Object);

            var supplier = new Supplier { SupplierName = "New" };

            var result = controller.Create(supplier);

            Assert.IsType<OkResult>(result);
            serviceMock.Verify(s => s.AddSupplier(supplier), Times.Once);
        }

        [Fact]
        public void Update_CallsService_AndReturnsOk()
        {
            var serviceMock = new Mock<ISupplierService>();
            var controller = new SupplierController(serviceMock.Object);

            var supplier = new Supplier { SupplierID = 3, SupplierName = "U" };

            var result = controller.Update(supplier);

            Assert.IsType<OkResult>(result);
            serviceMock.Verify(s => s.UpdateSupplier(supplier), Times.Once);
        }

        [Fact]
        public void Delete_CallsService_AndReturnsOk()
        {
            var serviceMock = new Mock<ISupplierService>();
            var controller = new SupplierController(serviceMock.Object);

            var result = controller.Delete(4);

            Assert.IsType<OkResult>(result);
            serviceMock.Verify(s => s.DeleteSupplier(4), Times.Once);
        }
    }
}
