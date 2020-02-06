using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Niles.PrintWeb.Api.Controllers;
using Niles.PrintWeb.Services;
using Niles.PrintWeb.Models.Entities;

namespace Niles.PrintWeb.Api.Tests
{
    [TestClass]
    public class TenantControllerTests
    {
        private readonly List<Tenant> tenants = new List<Tenant>
        {
            new Tenant {
                Name="vendor1",
                Id = 1,
                DateCreated = DateTime.Now,
                SubscribeDate = DateTime.Now,
                SubscriptionId = 1
            },
            new Tenant {
                Name="vendor2",
                Id = 2,
                DateCreated = DateTime.Now,
                SubscribeDate = DateTime.Now,
                SubscriptionId = 1
            }
        };

        [DataTestMethod]
        [DataRow("admin")]
        public async Task GetTest(string search)
        {
            TenantGetOptions options = new TenantGetOptions
            {
                Search = search
            };
            // Arrange
            var mock = new Mock<ITenantService>();
            mock.Setup(service => service.Get(options)).Returns(GetMock(options));
            var controller = new TenantController(mock.Object);

            // Act
            var actionResult = await controller.Get(options);
            var result = actionResult as OkObjectResult;

            // Assert
            Assert.IsInstanceOfType(actionResult, typeof(ActionResult));
            Assert.IsInstanceOfType(result, typeof(OkObjectResult));
            Assert.IsInstanceOfType(result.Value, typeof(IEnumerable<Tenant>));
        }

        [TestMethod]
        public async Task CreateSuccessTest()
        {
            // Arrange
            var mock = new Mock<ITenantService>();
            mock.Setup(service => service.Create(It.IsNotNull<Tenant>())).Returns(Task.FromResult(new Tenant()));
            var controller = new TenantController(mock.Object);

            // Act
            var actionResult = await controller.Create(new Tenant());
            var okResult = actionResult as OkObjectResult;

            // Assert
            Assert.IsInstanceOfType(actionResult, typeof(ActionResult));
            Assert.IsInstanceOfType(okResult, typeof(OkObjectResult));
            Assert.IsNotNull(okResult.Value);
        }

        [TestMethod]
        public async Task CreateError()
        {
            // Arrange
            var mock = new Mock<ITenantService>();
            mock.Setup(service => service.Create(It.IsAny<Tenant>())).Returns(Task.FromResult<Tenant>(null));
            var controller = new TenantController(mock.Object);

            // Act
            var actionResult = await controller.Create(new Tenant());
            var badResult = actionResult as BadRequestObjectResult;

            // Assert
            Assert.IsInstanceOfType(badResult, typeof(BadRequestObjectResult));
        }

        [TestMethod]
        public async Task CreateNotValid()
        {
            // Arrange
            var mock = new Mock<ITenantService>();
            mock.Setup(service => service.Validate(It.IsAny<TenantValidateOptions>())).Returns(Task.FromResult("Error"));
            var controller = new TenantController(mock.Object);

            // Act
            var actionResult = await controller.Create(new Tenant());
            var badResult = actionResult as BadRequestObjectResult;

            // Assert
            Assert.IsInstanceOfType(badResult, typeof(BadRequestObjectResult));
        }

        [TestMethod]
        public async Task UpdateSuccessTest()
        {
            // Arrange
            var mock = new Mock<ITenantService>();
            mock.Setup(service => service.Update(It.IsNotNull<Tenant>())).Returns(Task.FromResult(new Tenant()));
            var controller = new TenantController(mock.Object);

            // Act
            var actionResult = await controller.Update(new Tenant());
            var okResult = actionResult as OkObjectResult;

            // Assert
            Assert.IsInstanceOfType(actionResult, typeof(ActionResult));
            Assert.IsInstanceOfType(okResult, typeof(OkObjectResult));
            Assert.IsNotNull(okResult.Value);
        }

        [TestMethod]
        public async Task UpdateError()
        {
            // Arrange
            var mock = new Mock<ITenantService>();
            mock.Setup(service => service.Update(It.IsAny<Tenant>())).Returns(Task.FromResult<Tenant>(null));
            var controller = new TenantController(mock.Object);

            // Act
            var actionResult = await controller.Update(new Tenant());
            var badResult = actionResult as BadRequestObjectResult;

            // Assert
            Assert.IsInstanceOfType(badResult, typeof(BadRequestObjectResult));
        }

        [TestMethod]
        public async Task UpdateNotValid()
        {
            // Arrange
            var mock = new Mock<ITenantService>();
            mock.Setup(service => service.Validate(It.IsAny<TenantValidateOptions>())).Returns(Task.FromResult("Error"));
            var controller = new TenantController(mock.Object);

            // Act
            var actionResult = await controller.Update(new Tenant());
            var badResult = actionResult as BadRequestObjectResult;

            // Assert
            Assert.IsInstanceOfType(badResult, typeof(BadRequestObjectResult));
        }

        private Task<IEnumerable<Tenant>> GetMock(TenantGetOptions options)
        {
            IEnumerable<Tenant> result = tenants;

            if (options.Id.HasValue)
                result = result.Where(o => o.Id == options.Id);

            if (options.Ids != null && options.Ids.Count > 0)
                result = result.Where(o => options.Ids.Contains(o.Id));

            if (!string.IsNullOrEmpty(options.Search))
                result = result.Where(o => o.Name.ToLower().Contains(options.Search));

            return Task.FromResult(result);
        }
    }
}