using TaskManagement.Application.DTOs.Project;

namespace TaskManagement.Application.Interfaces;

public interface IProjectService
{
    Task<IEnumerable<ProjectDto>> GetAllAsync(Guid userId);
    Task<ProjectDto?> GetByIdAsync(Guid id, Guid userId);
    Task<ProjectDto> CreateAsync(CreateProjectDto dto, Guid userId);
    Task<ProjectDto?> UpdateAsync(UpdateProjectDto dto, Guid userId);
    Task<bool> DeleteAsync(Guid id, Guid userId);
}