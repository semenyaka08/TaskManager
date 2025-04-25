using Microsoft.AspNetCore.Mvc;
using TaskManager.Models;
using TaskManager.Repositories;

namespace TaskManager.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TaskNoteController(ITaskNoteRepository taskNoteRepository) : ControllerBase
{
    // GET: api/TaskNotes
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var notes = await taskNoteRepository.GetAllAsync();
        
        return Ok(notes);
    }

    // GET: api/TaskNotes/5
    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var note = await taskNoteRepository.GetByIdAsync(id);
        
        return note == null ? NotFound() : Ok(note);
    }

    // POST: api/TaskNotes
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] TaskNote note)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var created = await taskNoteRepository.CreateAsync(note);
        
        return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
    }

    // PUT: api/TaskNotes/5
    [HttpPut("{id}")]
    public async Task<IActionResult> Update([FromRoute] int id, [FromBody] TaskNote note)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var fetchedNote = await taskNoteRepository.GetByIdAsync(id);

        if (fetchedNote == null)
            return NotFound();

        fetchedNote.Title = note.Title;
        fetchedNote.Description = note.Description;

        await taskNoteRepository.SaveChangesAsync();

        return NoContent();
    }

    // DELETE: api/TaskNotes/5
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var fetchedNote = await taskNoteRepository.GetByIdAsync(id);

        if (fetchedNote == null)
            return NotFound();
        
        var deleted = await taskNoteRepository.DeleteAsync(fetchedNote);
        
        return deleted ? NoContent() : NotFound();
    }
}