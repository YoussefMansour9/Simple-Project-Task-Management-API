using TaskManagement.Application.DTOs.Task;

namespace TaskManagement.Application.Interfaces;

public interface ITaskService
{
    Task<IEnumerable<TaskDto>> GetByProjectIdAsync(Guid projectId, Guid userId);
    Task<TaskDto?> GetByIdAsync(Guid id, Guid userId);
    Task<TaskDto> CreateAsync(CreateTaskDto dto, Guid userId);
    Task<TaskDto?> UpdateAsync(UpdateTaskDto dto, Guid userId);
    Task<TaskDto?> UpdateStatusAsync(Guid id, string status, Guid userId);
    Task<bool> DeleteAsync(Guid id, Guid userId);
}