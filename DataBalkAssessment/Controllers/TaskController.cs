using DataBalkAssessment.Data;
using DataBalkAssessment.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Task = DataBalkAssessment.Models.Task;

namespace DataBalkAssessment.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Middleware.ApiKeyRequired]
    public class TaskController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public TaskController(ApplicationDbContext context)
        {
            _context = context;
        }

        // POST: api/Tasks
        [HttpPost("/AddTask")]
        public async Task<ActionResult<Task>> AddTask([FromBody] Task task)
        {
            var assignee = await _context.users.FindAsync(task.AssigneeId);
            if (assignee == null)
            {
                return NotFound("Assignee not found.");
            }

            if (string.IsNullOrEmpty(task.Title) || task.DueDate == default)
            {
                return BadRequest("Task Title and Due Date are required.");
            }
            _context.Tasks.Add(task);
            await _context.SaveChangesAsync();
            return CreatedAtAction("GetTask", new { id = task.Id }, task);
        }

         
        [HttpGet("/FindById/{id}")]
        public async Task<ActionResult<Task>> GetTask(int id)
        {
            var task = await _context.Tasks.FindAsync(id);

            if (task == null)
            {
                return NotFound();
            }

            return task;
        }



        [HttpGet("expired")]
        public async Task<ActionResult<IEnumerable<Task>>> GetExpiredTasks()
        {
            var expiredTasks = await _context.Tasks
                .Where(t => t.DueDate < DateTime.Now)
                .ToListAsync();

            return Ok(expiredTasks);
        }

        [HttpGet("active")]
        public async Task<ActionResult<IEnumerable<Task>>> GetActiveTasks()
        {
            var activeTasks = await _context.Tasks
                .Where(t => t.DueDate >= DateTime.Now)
                .ToListAsync();

            return Ok(activeTasks);
        }


        // GET: api/Tasks/fromCreatedDate/{date}
        [HttpGet("fromCreatedDate/{date}")]
        public async Task<ActionResult<IEnumerable<Task>>> GetTasksFromCreatedDate(DateTime date)
        {
           
            var tasksFromCreatedDate = await _context.Tasks
                .Where(t => t.CreatedDate.Date == date.Date) 
                .ToListAsync();

            return Ok(tasksFromCreatedDate);
        }

    }
}
