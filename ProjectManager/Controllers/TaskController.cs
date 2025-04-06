using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProjectManager.Models;

namespace ProjectManager.Controllers
{
    [Route("[controller]/[action]")]
    public class TasksController : Controller
    {
        private readonly ApplicationDbContext _context;

        public TasksController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpPost]
        public async Task<IActionResult> UpdateStatus([FromBody] TaskStatusUpdateModel model)
        {
            var task = await _context.TaskItems.FindAsync(model.Id);
            if (task == null) return NotFound();

            task.Status = model.Status;
            await _context.SaveChangesAsync();


            return Ok();
        }

        [HttpPost]
        public async Task<IActionResult> CreateTask(TaskItem task)
        {
            if (ModelState.IsValid)
            {
                _context.TaskItems.Add(task);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction("Details", "Projects", new { id = task.ProjectId });
        }

    }

    public class TaskStatusUpdateModel
    {
        public int Id { get; set; }
        public string Status { get; set; }
    }
}
