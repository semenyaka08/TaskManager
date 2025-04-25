using Microsoft.EntityFrameworkCore;
using TaskManager.Models;

namespace TaskManager.DataBase;

public class TaskNoteDbContext(DbContextOptions<TaskNoteDbContext> options) : DbContext(options)
{
    public DbSet<TaskNote> TaskNotes { get; set; }
}