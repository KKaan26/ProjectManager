using Microsoft.EntityFrameworkCore;


public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
    : base(options) { }

    public DbSet<Project> Projects { get; set; }

    public DbSet<TaskItem> TaskItems { get; set; }

}
