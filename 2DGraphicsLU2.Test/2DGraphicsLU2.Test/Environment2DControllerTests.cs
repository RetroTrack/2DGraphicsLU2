using _2DGraphicsLU2.WebApi.Controllers;
using _2DGraphicsLU2.WebApi.Models;
using _2DGraphicsLU2.WebApi.Repositories.Interfaces;
using _2DGraphicsLU2.WebApi.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace _2DGraphicsLU2.Test
{
    [TestClass]
    public class Environment2DControllerTests
    {
        [TestMethod]
        public async Task Add_AddEnvironmentToUserWithoutEnvironments_ReturnsCreatedEnvironment()
        {
            // ARRANGE
            var userId = Guid.NewGuid().ToString();
            var newEnvironment = GenerateRandomEnvironment("new environment");
            var existingUserEnvironments = GenerateRandomEnvironments(0);

            var environmentRepository = new Mock<IEnvironment2DRepository>();
            var authenticationService = new Mock<IAuthenticationService>();

            authenticationService.Setup(x => x.GetCurrentAuthenticatedUserId()).Returns(userId);
            environmentRepository.Setup(x => x.ReadAsync(userId)).ReturnsAsync(existingUserEnvironments);
            environmentRepository.Setup(x => x.InsertAsync(newEnvironment, userId)).ReturnsAsync(newEnvironment);

            var environmentController = new Environment2DController(authenticationService.Object, environmentRepository.Object);

            // ACT
            var response = await environmentController.Add(newEnvironment);

            // ASSERT

            // The OkObjectResult contains a value that is returned as the object to the client
            Assert.IsInstanceOfType(response, out CreatedResult createdResult);
            Assert.IsInstanceOfType(createdResult.Value, out Environment2D returnObject);
            Assert.AreEqual(newEnvironment.Id, returnObject.Id);
        }

        [TestMethod]
        public async Task Get_GetWithoutAuthenticatedUser_ReturnsBadRequest()
        {
            // Arrange

            var authenticationService = new Mock<IAuthenticationService>();
            var environmentRepository = new Mock<IEnvironment2DRepository>();
            authenticationService.Setup(x => x.GetCurrentAuthenticatedUserId()).Returns(value: null);

            var environmentController = new Environment2DController(authenticationService.Object, environmentRepository.Object);

            // Act
            var result = await environmentController.Get();

            // Assert
            Assert.IsInstanceOfType<BadRequestResult>(result.Result);
        }

        private List<Environment2D> GenerateRandomEnvironments(int numberOfEnvironments)
        {
            List<Environment2D> list = [];

            for (int i = 0; i < numberOfEnvironments; i++)
            {
                list.Add(GenerateRandomEnvironment($"Random Environment {i}"));
            }

            return list;
        }

        private Environment2D GenerateRandomEnvironment(string name)
        {
            var random = new Random();
            return new Environment2D
            {
                MaxHeight = random.Next(10, 100),
                MaxLength = random.Next(20, 200),
                Name = name ?? "Random environment"
            };
        }
    }
}
