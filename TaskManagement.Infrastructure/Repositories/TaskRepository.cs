using Microsoft.EntityFrameworkCore;
using TaskManagement.Domain.Entities;
using TaskManagement.Domain.Interfaces;
using TaskManagement.Infrastructure.Data;

namespace TaskManagement.Infrastructure.Repositories;

public class TaskRepository : ITaskRepository
{
    private readonly AppDbContext _context;

    public TaskRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<ProjectTask?> GetByIdAsync(Guid id)
    {
        return await _context.Tasks.FindAsync(id);
    }

    public async Task<ProjectTask?> GetByIdWithProjectAsync(Guid id)
    {
        return await _context.Tasks
            .Include(t => t.Project)
            .FirstOrDefaultAsync(t => t.Id == id);
    }

    public async Task<IEnumerable<ProjectTask>> GetAllAsync()
    {
        return await _context.Tasks
            .Include(t => t.Project)
            .ToListAsync();
    }

    public async Task<IEnumerable<ProjectTask>> GetByProjectIdAsync(Guid projectId)
    {
        return await _context.Tasks
            .Include(t => t.Project)
            .Where(t => t.ProjectId == projectId)
            .ToListAsync();
    }

    public async Task<ProjectTask> AddAsync(ProjectTask entity)
    {
        await _context.Tasks.AddAsync(entity);
        return entity;
    }

    public Task UpdateAsync(ProjectTask entity)
    {
        _context.Tasks.Update(entity);
        return Task.CompletedTask;
    }

    public async Task DeleteAsync(Guid id)
    {
        var task = await _context.Tasks.FindAsync(id);
        if (task != null)
        {
            _context.Tasks.Remove(task);
        }
    }

    public async Task<int> SaveChangesAsync()
    {
        return await _context.SaveChangesAsync();
    }
}