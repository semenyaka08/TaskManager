using Microsoft.EntityFrameworkCore;
using TaskManager.DataBase;
using TaskManager.Models;

namespace TaskManager.Repositories;

public class TaskNoteRepository(TaskNoteDbContext context) : ITaskNoteRepository
{
    public async Task<IEnumerable<TaskNote>> GetAllAsync()
    {
        return await context.TaskNotes.OrderByDescending(n => n.CreatedAt).ToListAsync();
    }

    public async Task<TaskNote?> GetByIdAsync(int id)
    {
        return await context.TaskNotes.FindAsync(id);
    }

    public async Task<TaskNote> CreateAsync(TaskNote note)
    {
        await context.TaskNotes.AddAsync(note);
        
        await context.SaveChangesAsync();
        
        return note;
    }

    public async Task<bool> UpdateAsync(TaskNote note)
    {
        context.TaskNotes.Update(note);
        
        return await context.SaveChangesAsync() > 0;
    }

    public async Task<bool> DeleteAsync(TaskNote note)
    {
        context.TaskNotes.Remove(note);

        return await context.SaveChangesAsync() > 0;
    }

    public async Task SaveChangesAsync()
    {
        await context.SaveChangesAsync();
    }
}