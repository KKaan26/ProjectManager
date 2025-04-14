using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

public class ProjectsController : Controller
{
    private readonly ApplicationDbContext _context;

    public ProjectsController(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IActionResult> Index()
    {
        return View(await _context.Projects.ToListAsync());
    }

    public IActionResult Create()
    {
        return View();
    }
    [HttpPost]
    public async Task<IActionResult> CreateTask(TaskItem task)
    {
        // Çünkü formdan sadece ProjectId geliyor, Project objesi null
        ModelState.Remove("Project");

        if (ModelState.IsValid)
        {
            _context.TaskItems.Add(task);
            await _context.SaveChangesAsync();

            return RedirectToAction("Details", "Projects", new { id = task.ProjectId });
        }

        // Hataları loglamaya devam
        foreach (var value in ModelState.Values)
        {
            foreach (var error in value.Errors)
            {
                Console.WriteLine("Model error: " + error.ErrorMessage);
            }
        }

        return BadRequest(ModelState);
    }



    [HttpPost]
    public async Task<IActionResult> Create(Project project)
    {
        if (ModelState.IsValid)
        {
            _context.Add(project);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        return View(project);
    }

    public async Task<IActionResult> Edit(int id)
    {
        var project = await _context.Projects.FindAsync(id);
        if (project == null) return NotFound();
        return View(project);
    }

    [HttpPost]
    public async Task<IActionResult> Edit(int id, Project project)
    {
        if (id != project.Id) return NotFound();

        if (ModelState.IsValid)
        {
            _context.Update(project);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        return View(project);
    }

    public async Task<IActionResult> Delete(int id)
    {
        var project = await _context.Projects.FindAsync(id);
        if (project == null) return NotFound();
        return View(project);
    }

    [HttpPost, ActionName("Delete")]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var project = await _context.Projects.FindAsync(id);
        _context.Projects.Remove(project);
        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> Details(int id)
    {
        var project = await _context.Projects
            .Include(p => p.Tasks)
            .FirstOrDefaultAsync(p => p.Id == id);

        if (project == null) return NotFound();

        return View(project);
    }

    [HttpPost]
    public async Task<IActionResult> UpdateTask(TaskItem updatedTask)
    {
        var task = await _context.TaskItems.FindAsync(updatedTask.Id);
        if (task == null) return NotFound();

        task.Description = updatedTask.Description;
        task.StartDate = updatedTask.StartDate;
        task.EstimatedEndDate = updatedTask.EstimatedEndDate;

        await _context.SaveChangesAsync();

        return RedirectToAction("Details", "Projects", new { id = updatedTask.ProjectId });
    }

    [HttpPost]
    [Route("Tasks/DeleteTask/{id}")]
    public async Task<IActionResult> DeleteTask(int id)
    {
        var task = await _context.TaskItems.FindAsync(id);
        if (task == null) return NotFound();

        _context.TaskItems.Remove(task);
        await _context.SaveChangesAsync();

        return Ok();
    }



}
