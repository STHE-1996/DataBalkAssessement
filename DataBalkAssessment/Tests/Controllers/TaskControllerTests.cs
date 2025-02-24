using DataBalkAssessment.Controllers;
using DataBalkAssessment.Models;
using DataBalkAssessment.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using Xunit;
using System.Threading.Tasks;
using System;
using System.Linq;

namespace DataBalkAssessment.Tests
{
    public class TaskControllerTests
    {
        private readonly TaskController _controller;
        private readonly ApplicationDbContext _context;

        public TaskControllerTests()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: "TestDb")
                .Options;

            _context = new ApplicationDbContext(options);

            _context.users.Add(new User { Id = 1, FirstName = "Sthembiso" });
            _context.SaveChanges();

            _controller = new TaskController(_context);
        }

        
        [Fact]
        public async System.Threading.Tasks.Task AddTask_ShouldReturnCreatedResult_WhenTaskIsValid()
        {
            
            var task = new DataBalkAssessment.Models.Task
            {
                Title = "Valid Task",
                AssigneeId = 1, 
                DueDate = DateTime.Now.AddDays(1),
                CreatedDate = DateTime.Now
            };

            
            var result = await _controller.AddTask(task);

            
            var createdResult = Assert.IsType<CreatedAtActionResult>(result);
            var returnValue = Assert.IsType<DataBalkAssessment.Models.Task>(createdResult.Value);
            Assert.Equal(task.Title, returnValue.Title);
        }

        
        [Fact]
        public async System.Threading.Tasks.Task AddTask_ShouldReturnNotFound_WhenAssigneeDoesNotExist()
        {
            var task = new DataBalkAssessment.Models.Task
            {
                Title = "Test Task",
                AssigneeId = 99, 
                DueDate = DateTime.Now.AddDays(1),
                CreatedDate = DateTime.Now
            };


            var result = await _controller.AddTask(task);

            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
            Assert.Equal("Assignee not found.", notFoundResult.Value);
        }
    }
}
