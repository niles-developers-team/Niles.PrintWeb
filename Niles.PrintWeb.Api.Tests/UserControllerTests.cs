using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Niles.PrintWeb.Api.Controllers;
using Niles.PrintWeb.Api.Services.Interfaces;
using Niles.PrintWeb.Models.Entities;

namespace Niles.PrintWeb.Api.Tests
{
    [TestClass]
    public class UserControllerTests
    {
        private readonly List<User> users = new List<User>
        {
            new User {
                Email = "test@email.com",
                Confirmed = true,
                FirstName = "admin",
                Id = 1,
                LastName = "admin",
                Password = "admin",
                UserName = "admin"
            },
            new User {
                Email = "test1@email.com",
                Confirmed = true,
                FirstName = "notadmin",
                Id = 2,
                LastName = "notadmin",
                Password = "notadmin",
                UserName = "notadmin"
            }
        };

        [DataTestMethod]
        [DataRow("admin")]
        public async Task GetTest(string search)
        {
            UserGetOptions options = new UserGetOptions
            {
                Search = search
            };
            // Arrange
            var mock = new Mock<IUserService>();
            mock.Setup(service => service.Get(options)).Returns(GetMock(options));
            var controller = new UserController(mock.Object);

            // Act
            var actionResult = await controller.Get(options);
            var result = actionResult as OkObjectResult;

            // Assert
            Assert.IsInstanceOfType(actionResult, typeof(ActionResult));
            Assert.IsInstanceOfType(result, typeof(OkObjectResult));
            Assert.IsInstanceOfType(result.Value, typeof(IEnumerable<User>));
        }

        [TestMethod]
        public async Task CreateSuccessTest()
        {
            // Arrange
            var mock = new Mock<IUserService>();
            mock.Setup(service => service.Create(It.IsNotNull<UserAuthenticated>())).Returns(Task.FromResult(new User()));
            var controller = new UserController(mock.Object);

            // Act
            var actionResult = await controller.Create(new UserAuthenticated());
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
            var mock = new Mock<IUserService>();
            mock.Setup(service => service.Create(It.IsAny<UserAuthenticated>())).Returns(Task.FromResult<User>(null));
            var controller = new UserController(mock.Object);

            // Act
            var actionResult = await controller.Create(new UserAuthenticated());
            var badResult = actionResult as BadRequestObjectResult;

            // Assert
            Assert.IsInstanceOfType(badResult, typeof(BadRequestObjectResult));
        }

        [TestMethod]
        public async Task CreateNotValid()
        {
            // Arrange
            var mock = new Mock<IUserService>();
            mock.Setup(service => service.Validate(It.IsAny<UserGetOptions>())).Returns(Task.FromResult("Error"));
            var controller = new UserController(mock.Object);

            // Act
            var actionResult = await controller.Create(new UserAuthenticated());
            var badResult = actionResult as BadRequestObjectResult;

            // Assert
            Assert.IsInstanceOfType(badResult, typeof(BadRequestObjectResult));
        }

        [TestMethod]
        public async Task UpdateSuccessTest()
        {
            // Arrange
            var mock = new Mock<IUserService>();
            mock.Setup(service => service.Update(It.IsNotNull<User>())).Returns(Task.FromResult(new User()));
            var controller = new UserController(mock.Object);

            // Act
            var actionResult = await controller.Update(new User());
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
            var mock = new Mock<IUserService>();
            mock.Setup(service => service.Update(It.IsAny<User>())).Returns(Task.FromResult<User>(null));
            var controller = new UserController(mock.Object);

            // Act
            var actionResult = await controller.Update(new User());
            var badResult = actionResult as BadRequestObjectResult;

            // Assert
            Assert.IsInstanceOfType(badResult, typeof(BadRequestObjectResult));
        }

        [TestMethod]
        public async Task UpdateNotValid()
        {
            // Arrange
            var mock = new Mock<IUserService>();
            mock.Setup(service => service.Validate(It.IsAny<UserGetOptions>())).Returns(Task.FromResult("Error"));
            var controller = new UserController(mock.Object);

            // Act
            var actionResult = await controller.Update(new User());
            var badResult = actionResult as BadRequestObjectResult;

            // Assert
            Assert.IsInstanceOfType(badResult, typeof(BadRequestObjectResult));
        }

        [DataTestMethod]
        public async Task ConfirmTest()
        {
            // Arrange
            var mock = new Mock<IUserService>();
            mock.Setup(service => service.Confirm(It.IsAny<Guid>())).Returns(Task.CompletedTask);
            var controller = new UserController(mock.Object);

            // Act
            var actionResult = await controller.Confirm(It.IsAny<Guid>());
            var result = actionResult as OkResult;

            // Assert
            Assert.IsInstanceOfType(actionResult, typeof(ActionResult));
            Assert.IsInstanceOfType(result, typeof(OkResult));
        }

        [TestMethod]
        public async Task SignInSuccessTest()
        {
            // Arrange
            var mock = new Mock<IUserService>();
            mock.Setup(service => service.SignIn(It.IsNotNull<UserGetOptions>())).Returns(Task.FromResult(new UserAuthenticated()));
            var controller = new UserController(mock.Object);

            // Act
            var actionResult = await controller.SignIn(new UserGetOptions());
            var result = actionResult as OkObjectResult;

            // Assert
            Assert.IsInstanceOfType(actionResult, typeof(ActionResult));
            Assert.IsInstanceOfType(result, typeof(OkObjectResult));
            Assert.IsNotNull(result.Value);
        }

        [TestMethod]
        public async Task SignInErrorTest()
        {
            // Arrange
            var mock = new Mock<IUserService>();
            mock.Setup(service => service.SignIn(It.IsNotNull<UserGetOptions>())).Returns(Task.FromResult<UserAuthenticated>(null));
            var controller = new UserController(mock.Object);

            // Act
            var actionResult = await controller.SignIn(new UserGetOptions());
            var result = actionResult as BadRequestObjectResult;

            // Assert
            Assert.IsInstanceOfType(actionResult, typeof(ActionResult));
            Assert.IsInstanceOfType(result, typeof(BadRequestObjectResult));
            Assert.IsNotNull(result.Value);
        }

        private Task<IEnumerable<User>> GetMock(UserGetOptions options)
        {
            IEnumerable<User> result = users;

            if (!string.IsNullOrEmpty(options.Email))
                result = result.Where(o => o.Email == options.Email);

            if (options.Id.HasValue)
                result = result.Where(o => o.Id == options.Id);

            if (options.Ids != null && options.Ids.Count > 0)
                result = result.Where(o => options.Ids.Contains(o.Id));

            if(options.OnlyConfirmed)
                result = result.Where(o => o.Confirmed);

            if(!string.IsNullOrEmpty(options.Password))
                result = result.Where(o => o.Password == options.Password);

            if(!string.IsNullOrEmpty(options.Search))
                result = result.Where(o => o.UserName.ToLower().Contains(options.Search));

            if(!string.IsNullOrEmpty(options.UserName))
                result = result.Where(o => o.UserName == options.UserName);            

            return Task.FromResult(result);
        }
    }
}