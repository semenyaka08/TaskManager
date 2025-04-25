using Microsoft.AspNetCore.Mvc;
using Moq;
using TaskManager.Controllers;
using TaskManager.Models;
using TaskManager.Repositories;

namespace TaskManager.Tests;

public class TaskNoteControllerTests
{
    private readonly Mock<ITaskNoteRepository> _taskNoteRepositoryMock;
    private readonly TaskNoteController _controller;

    public TaskNoteControllerTests()
    {
        _taskNoteRepositoryMock = new Mock<ITaskNoteRepository>();
        _controller = new TaskNoteController(_taskNoteRepositoryMock.Object);
    }

    [Fact]
    public async Task GetAll_ReturnsOkResult_WithListOfTaskNotes()
    {
        // Arrange
        var mockNotes = new List<TaskNote>
        {
            new TaskNote { Id = 1, Title = "Test Task 1", Description = "Description 1" },
            new TaskNote { Id = 2, Title = "Test Task 2", Description = "Description 2" }
        };

        _taskNoteRepositoryMock.Setup(repo => repo.GetAllAsync()).ReturnsAsync(mockNotes);

        // Act
        var result = await _controller.GetAll();

        // Assert
        var actionResult = Assert.IsType<OkObjectResult>(result);
        var returnValue = Assert.IsAssignableFrom<List<TaskNote>>(actionResult.Value);
        Assert.Equal(mockNotes.Count, returnValue.Count);
    }

    [Fact]
    public async Task GetById_ReturnsNotFound_WhenTaskNoteDoesNotExist()
    {
        // Arrange
        _taskNoteRepositoryMock.Setup(repo => repo.GetByIdAsync(1)).ReturnsAsync((TaskNote)null);

        // Act
        var result = await _controller.GetById(1);

        // Assert
        Assert.IsType<NotFoundResult>(result);
    }

    [Fact]
    public async Task GetById_ReturnsOkResult_WhenTaskNoteExists()
    {
        // Arrange
        var mockNote = new TaskNote { Id = 1, Title = "Test Task", Description = "Description" };
        _taskNoteRepositoryMock.Setup(repo => repo.GetByIdAsync(1)).ReturnsAsync(mockNote);

        // Act
        var result = await _controller.GetById(1);

        // Assert
        var actionResult = Assert.IsType<OkObjectResult>(result);
        var returnValue = Assert.IsType<TaskNote>(actionResult.Value);
        Assert.Equal(mockNote.Id, returnValue.Id);
    }

    [Fact]
    public async Task Create_ReturnsBadRequest_WhenModelStateIsInvalid()
    {
        // Arrange
        _controller.ModelState.AddModelError("Title", "Required");

        // Act
        var result = await _controller.Create(new TaskNote());

        // Assert
        Assert.IsType<BadRequestObjectResult>(result);
    }

    [Fact]
    public async Task Create_ReturnsCreatedAtAction_WhenTaskNoteIsCreated()
    {
        // Arrange
        var newNote = new TaskNote { Title = "New Task", Description = "New Description" };
        var createdNote = new TaskNote { Id = 1, Title = "New Task", Description = "New Description" };
        _taskNoteRepositoryMock.Setup(repo => repo.CreateAsync(It.IsAny<TaskNote>())).ReturnsAsync(createdNote);

        // Act
        var result = await _controller.Create(newNote);

        // Assert
        var actionResult = Assert.IsType<CreatedAtActionResult>(result);
        var returnValue = Assert.IsType<TaskNote>(actionResult.Value);
        Assert.Equal(createdNote.Id, returnValue.Id);
    }

    [Fact]
    public async Task Update_ReturnsBadRequest_WhenModelStateIsInvalid()
    {
        // Arrange
        _controller.ModelState.AddModelError("Title", "Required");

        // Act
        var result = await _controller.Update(1, new TaskNote());

        // Assert
        Assert.IsType<BadRequestObjectResult>(result);
    }

    [Fact]
    public async Task Update_ReturnsNotFound_WhenTaskNoteDoesNotExist()
    {
        // Arrange
        _taskNoteRepositoryMock.Setup(repo => repo.GetByIdAsync(1)).ReturnsAsync((TaskNote)null);

        // Act
        var result = await _controller.Update(1, new TaskNote());

        // Assert
        Assert.IsType<NotFoundResult>(result);
    }

    [Fact]
    public async Task Update_ReturnsNoContent_WhenTaskNoteIsUpdated()
    {
        // Arrange
        var existingNote = new TaskNote { Id = 1, Title = "Old Title", Description = "Old Description" };
        var updatedNote = new TaskNote { Title = "New Title", Description = "New Description" };
        _taskNoteRepositoryMock.Setup(repo => repo.GetByIdAsync(1)).ReturnsAsync(existingNote);

        // Act
        var result = await _controller.Update(1, updatedNote);

        // Assert
        Assert.IsType<NoContentResult>(result);
    }

    [Fact]
    public async Task Delete_ReturnsNotFound_WhenTaskNoteDoesNotExist()
    {
        // Arrange
        _taskNoteRepositoryMock.Setup(repo => repo.GetByIdAsync(1)).ReturnsAsync((TaskNote)null);

        // Act
        var result = await _controller.Delete(1);

        // Assert
        Assert.IsType<NotFoundResult>(result);
    }

    [Fact]
    public async Task Delete_ReturnsNoContent_WhenTaskNoteIsDeleted()
    {
        // Arrange
        var existingNote = new TaskNote { Id = 1, Title = "Task to delete", Description = "To be deleted" };
        _taskNoteRepositoryMock.Setup(repo => repo.GetByIdAsync(1)).ReturnsAsync(existingNote);
        _taskNoteRepositoryMock.Setup(repo => repo.DeleteAsync(existingNote)).ReturnsAsync(true);

        // Act
        var result = await _controller.Delete(1);

        // Assert
        Assert.IsType<NoContentResult>(result);
    }
}