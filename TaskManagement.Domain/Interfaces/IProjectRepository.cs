using TaskManagement.Domain.Entities;

namespace TaskManagement.Domain.Interfaces;

public interface IProjectRepository : IRepository<Project>
{
    Task<Project?> GetByIdWithTasksAsync(Guid id);
    Task<IEnumerable<Project>> GetByOwnerIdAsync(Guid ownerId);
}