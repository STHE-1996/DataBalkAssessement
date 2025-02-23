using Moq;
using Xunit;
using Microsoft.AspNetCore.Mvc;
using DataBalkAssessment.Controllers;
using DataBalkAssessment.Data;
using DataBalkAssessment.Models;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using FluentAssertions;

namespace DataBalkAssessment.Tests
{
    public class UserControllerTests
    {
        private readonly UserController _controller;
        private readonly Mock<ApplicationDbContext> _mockContext;

        public UserControllerTests()
        {
            // Setup in-memory database for unit tests
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: "TestDb")
                .Options;

            _mockContext = new Mock<ApplicationDbContext>(options);
            _controller = new UserController(_mockContext.Object);
        }

        #region Test Authenticate Method

        [Fact]
        public async System.Threading.Tasks.Task Authenticate_ReturnsBadRequest_WhenUserObjIsNull()
        {
            // Arrange
            RegisterRequest userObj = null;

            // Act
            var result = await _controller.Authenticate(userObj);

            // Assert
            result.Should().BeOfType<BadRequestResult>();
        }

        [Fact]
        public async System.Threading.Tasks.Task Authenticate_ReturnsNotFound_WhenUserDoesNotExist()
        {
            // Arrange
            var userObj = new RegisterRequest
            {
                Email = "sthembisobuthelezi774@gmail.com",
                Password = "sthembiso123"
            };

            // Act
            var result = await _controller.Authenticate(userObj);

            // Assert
            result.Should().BeOfType<NotFoundObjectResult>();
        }

        [Fact]
        public async System.Threading.Tasks.Task Authenticate_ReturnsOk_WhenUserIsFound()
        {
            // Arrange
            var userObj = new RegisterRequest
            {
                Email = "sthembisobuthelezi774@gmail.com",
                Password = "sthembiso123"
            };

            var existingUser = new User
            {
                Email = "sthembisobuthelezi774@gmail.com",
                Password = "sthembiso123"
            };

            _mockContext.Setup(c => c.users.FirstOrDefaultAsync(It.IsAny<System.Linq.Expressions.Expression<Func<User, bool>>>(), default))
                .ReturnsAsync(existingUser);

            // Act
            var result = await _controller.Authenticate(userObj);

            // Assert
            var okResult = result.Should().BeOfType<OkObjectResult>().Subject;
            var returnValue = okResult.Value as dynamic;
            returnValue.Message.Should().Be("Successful Login!");
        }

        #endregion

        #region Test RegisterUser Method

        [Fact]
        public async System.Threading.Tasks.Task RegisterUser_ReturnsBadRequest_WhenUserObjIsNull()
        {
            // Arrange
            RegisterRequest userObj = null;

            // Act
            var result = await _controller.RegisterUser(userObj);

            // Assert
            result.Should().BeOfType<BadRequestResult>();
        }

        [Fact]
        public async System.Threading.Tasks.Task RegisterUser_ReturnsOk_WhenUserIsRegistered()
        {
            // Arrange
            var userObj = new RegisterRequest
            {
                FirstName = "sthe",
                LastName = "buthelezi",
                Email = "sthembisobuthelezi774@gmail.com",
                Password = "sthembiso123",
                ConfirmPassword = "sthembiso123",
                Role = "User",
                PhoneNumber = "0636015960"
            };

            // Act
            var result = await _controller.RegisterUser(userObj);

            // Assert
            var okResult = result.Should().BeOfType<OkObjectResult>().Subject;
            var returnValue = okResult.Value as dynamic;
            returnValue.Message.Should().Be("User Registered!");
            returnValue.ApiKey.Should().NotBeNullOrEmpty();
        }

        #endregion
    }
}
