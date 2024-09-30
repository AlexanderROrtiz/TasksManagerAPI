using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System.Dynamic;
using System.Security.Claims;
using TaskManagement.Api.Controllers;
using TaskManagement.Application.DTOs.TasksDTOs;
using TaskManagement.Application.Interfaces;
using TaskManagement.Application.Responses;
using TaskManagement.Domain.Entities;

namespace TaskManagement.Api.Test.Controllers
{
    public class TaskControllerTests
    {
        private readonly Mock<ITaskService> _mockTaskService;
        private readonly TaskController _controller;

        public TaskControllerTests()
        {
            _mockTaskService = new Mock<ITaskService>();
            _controller = new TaskController(_mockTaskService.Object);
        }

        private void SetupUser(string role)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, "1"),
                new Claim(ClaimTypes.Role, role)
            };
            var identity = new ClaimsIdentity(claims, "TestAuthType");
            _controller.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext { User = new ClaimsPrincipal(identity) }
            };
        }

        [Fact]
        public async Task GetAllTasksAsync_TasksFound()
        {
            SetupUser("Administrador");

            var tasks = new List<TaskDto>
            {
                new TaskDto { Id = 1, Title = "Task 1" },
                new TaskDto { Id = 2, Title = "Task 2" }
            };

            _mockTaskService.Setup(s => s.GetAllTasksAsync()).ReturnsAsync(tasks);

            // Act  
            var result = await _controller.GetAllTasksAsync();

            // Assert  
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnValue = Assert.IsType<TaskResponse>(okResult.Value);  

            Assert.Equal("Tareas encontradas correctamente.", returnValue.Message);
            Assert.Equal(tasks.Count, returnValue.Tasks.Count);
        }

        [Fact]
        public async Task GetAllTasksAsync_NoTasksFound()
        {
            SetupUser("Administrador");

            _mockTaskService.Setup(s => s.GetAllTasksAsync()).ReturnsAsync(new List<TaskDto>());

            // Act  
            var result = await _controller.GetAllTasksAsync();

            // Assert  
            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
            var returnValue = Assert.IsType<NotFoundResponse>(notFoundResult.Value);

            Assert.Equal("No se encontraron tareas.", returnValue.Message);
        }

        [Fact]
        public void VerifyToken_UserIdIsInvalid()
        {
            // Arrange
            var user = new ClaimsPrincipal(new ClaimsIdentity()); 
            _controller.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext { User = user }
            };

            // Act
            var result = _controller.VerifyToken();

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal("El ID de usuario no se encuentra o es inválido.", badRequestResult.Value);
        }
    }
}