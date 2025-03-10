using _2DGraphicsLU2.WebApi.Controllers;
using _2DGraphicsLU2.WebApi.Models;
using _2DGraphicsLU2.WebApi.Repositories.Interfaces;
using _2DGraphicsLU2.WebApi.Services;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace _2DGraphicsLU2.Test
{
    [TestClass]
    public class Object2DControllerTests
    {
        [TestMethod]
        public async Task Get_GetWithObject_ReturnsOkResult()
        {
            // Arrange
            var userId = Guid.NewGuid().ToString();
            var environmentId = Guid.NewGuid();
            var objectId = Guid.NewGuid();

            var authenticationService = new Mock<IAuthenticationService>();
            var objectRepository = new Mock<IObject2DRepository>();

            var mockObject2D = new Object2D { Id = objectId, PrefabId = "TestPrefab" };

            authenticationService.Setup(x => x.GetCurrentAuthenticatedUserId()).Returns(userId);
            objectRepository.Setup(x => x.ReadAsync(environmentId, objectId, userId)).ReturnsAsync(mockObject2D);

            var objectController = new Object2DController(authenticationService.Object, objectRepository.Object);

            // Act
            var result = await objectController.Get(environmentId, objectId);

            // Assert
            Assert.IsInstanceOfType(result.Result, out OkObjectResult okResult);
            Assert.IsInstanceOfType(okResult.Value, out Object2D returnObject);
            Assert.AreEqual(objectId, returnObject.Id);
        }

        [TestMethod]
        public async Task Get_ReturnsNotFound_WhenObjectDoesNotExist()
        {
            // Arrange
            var userId = Guid.NewGuid().ToString();
            var environmentId = Guid.NewGuid();
            var objectId = Guid.NewGuid();

            
            var authenticationService = new Mock<IAuthenticationService>();
            var objectRepository = new Mock<IObject2DRepository>();

            var mockObject2D = new Object2D { Id = objectId, PrefabId = "TestPrefab" };

            authenticationService.Setup(x => x.GetCurrentAuthenticatedUserId()).Returns(userId);
            objectRepository.Setup(x => x.ReadAsync(environmentId, objectId, userId)).ReturnsAsync(value: null);

            var objectController = new Object2DController(authenticationService.Object, objectRepository.Object);

            // Act
            var result = await objectController.Get(environmentId, objectId);

            // Assert
            Assert.IsInstanceOfType<NotFoundResult>(result.Result);
        }

        [TestMethod]
        public async Task Delete_ReturnsOkResult_WhenObjectIsDeleted()
        {
            // Arrange
            var userId = Guid.NewGuid().ToString();
            var environmentId = Guid.NewGuid();
            var objectId = Guid.NewGuid();
            var existingObject = new Object2D { Id = objectId, PrefabId = "TestPrefab" };

            var authenticationService = new Mock<IAuthenticationService>();
            var objectRepository = new Mock<IObject2DRepository>();

            authenticationService.Setup(x => x.GetCurrentAuthenticatedUserId()).Returns(userId);
            objectRepository.Setup(x => x.ReadAsync(environmentId, objectId, userId)).ReturnsAsync(existingObject);

            objectRepository.Setup(x => x.ReadAsync(environmentId, objectId, userId)).ReturnsAsync(existingObject);

            authenticationService.Setup(s => s.GetCurrentAuthenticatedUserId()).Returns(userId);
            objectRepository.Setup(r => r.ReadAsync(environmentId, objectId, userId)).ReturnsAsync(existingObject);
            objectRepository.Setup(r => r.DeleteAsync(environmentId, objectId, userId)).Returns(Task.CompletedTask);

            var objectController = new Object2DController(authenticationService.Object, objectRepository.Object);
            // Act
            var result = await objectController.Delete(environmentId, objectId);

            // Assert
            Assert.IsInstanceOfType<OkResult>(result);
        }
    }
}
