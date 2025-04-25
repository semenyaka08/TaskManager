using TaskManager.Models;

namespace TaskManager.Repositories;

public interface ITaskNoteRepository
{
    Task<IEnumerable<TaskNote>> GetAllAsync();
    
    Task<TaskNote?> GetByIdAsync(int id);
    
    Task<TaskNote> CreateAsync(TaskNote note);
    
    Task<bool> DeleteAsync(TaskNote note);

    Task SaveChangesAsync();
}