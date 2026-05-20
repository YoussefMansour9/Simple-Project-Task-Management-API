using AutoMapper;
using FluentResults;
using TaskManagement.Application.DTOs.Task;
using TaskManagement.Application.Interfaces;
using TaskManagement.Domain.Entities;
using TaskManagement.Domain.Enums;
using TaskManagement.Domain.Interfaces;

namespace TaskManagement.Infrastructure.Services;

public class TaskService : ITaskService
{
    private readonly ITaskRepository _taskRepository;
    private readonly IProjectRepository _projectRepository;
    private readonly IMapper _mapper;

    public TaskService(ITaskRepository taskRepository, IProjectRepository projectRepository, IMapper mapper)
    {
        _taskRepository = taskRepository;
        _projectRepository = projectRepository;
        _mapper = mapper;
    }

    public async Task<IEnumerable<TaskDto>> GetByProjectIdAsync(Guid projectId, Guid userId)
    {
        var project = await _projectRepository.GetByIdAsync(projectId);
        if (project == null || project.OwnerId != userId)
            return Enumerable.Empty<TaskDto>();

        var tasks = await _taskRepository.GetByProjectIdAsync(projectId);
        return _mapper.Map<IEnumerable<TaskDto>>(tasks);
    }

    public async Task<TaskDto?> GetByIdAsync(Guid id, Guid userId)
    {
        var task = await _taskRepository.GetByIdWithProjectAsync(id);
        if (task == null || task.Project.OwnerId != userId)
            return null;
        return _mapper.Map<TaskDto>(task);
    }

    public async Task<TaskDto> CreateAsync(CreateTaskDto dto, Guid userId)
    {
        var project = await _projectRepository.GetByIdAsync(dto.ProjectId);
        if (project == null || project.OwnerId != userId)
            throw new InvalidOperationException("Project not found or access denied");

        var task = new ProjectTask
        {
            Id = Guid.NewGuid(),
            Title = dto.Title,
            Description = dto.Description,
            DueDate = dto.DueDate,
            Priority = dto.Priority,
            ProjectId = dto.ProjectId,
            CreatedByUserId = userId,
            CreatedAt = DateTime.UtcNow
        };

        await _taskRepository.AddAsync(task);
        await _taskRepository.SaveChangesAsync();

        return _mapper.Map<TaskDto>(task);
    }

    public async Task<TaskDto?> UpdateAsync(UpdateTaskDto dto, Guid userId)
    {
        var task = await _taskRepository.GetByIdWithProjectAsync(dto.Id);
        if (task == null || task.Project.OwnerId != userId)
            return null;

        task.Title = dto.Title;
        task.Description = dto.Description;
        task.Status = dto.Status;
        task.DueDate = dto.DueDate;
        task.Priority = dto.Priority;
        task.UpdatedAt = DateTime.UtcNow;

        await _taskRepository.UpdateAsync(task);
        await _taskRepository.SaveChangesAsync();

        return _mapper.Map<TaskDto>(task);
    }

    public async Task<TaskDto?> UpdateStatusAsync(Guid id, string status, Guid userId)
    {
        var task = await _taskRepository.GetByIdWithProjectAsync(id);
        if (task == null || task.Project.OwnerId != userId)
            return null;

        if (!Enum.TryParse<Domain.Enums.TaskStatus>(status, true, out var newStatus))
            throw new ArgumentException("Invalid status value");

        task.Status = newStatus;
        task.UpdatedAt = DateTime.UtcNow;

        await _taskRepository.UpdateAsync(task);
        await _taskRepository.SaveChangesAsync();

        return _mapper.Map<TaskDto>(task);
    }

    public async Task<bool> DeleteAsync(Guid id, Guid userId)
    {
        var task = await _taskRepository.GetByIdWithProjectAsync(id);
        if (task == null || task.Project.OwnerId != userId)
            return false;

        await _taskRepository.DeleteAsync(id);
        await _taskRepository.SaveChangesAsync();
        return true;
    }
}