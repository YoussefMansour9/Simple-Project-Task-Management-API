using AutoMapper;
using FluentResults;
using TaskManagement.Application.DTOs.Project;
using TaskManagement.Application.Interfaces;
using TaskManagement.Domain.Entities;
using TaskManagement.Domain.Interfaces;

namespace TaskManagement.Infrastructure.Services;

public class ProjectService : IProjectService
{
    private readonly IProjectRepository _projectRepository;
    private readonly IUserRepository _userRepository;
    private readonly IMapper _mapper;

    public ProjectService(IProjectRepository projectRepository, IUserRepository userRepository, IMapper mapper)
    {
        _projectRepository = projectRepository;
        _userRepository = userRepository;
        _mapper = mapper;
    }

    public async Task<IEnumerable<ProjectDto>> GetAllAsync(Guid userId)
    {
        var projects = await _projectRepository.GetByOwnerIdAsync(userId);
        return _mapper.Map<IEnumerable<ProjectDto>>(projects);
    }

    public async Task<ProjectDto?> GetByIdAsync(Guid id, Guid userId)
    {
        var project = await _projectRepository.GetByIdWithTasksAsync(id);
        if (project == null || project.OwnerId != userId)
            return null;
        return _mapper.Map<ProjectDto>(project);
    }

    public async Task<ProjectDto> CreateAsync(CreateProjectDto dto, Guid userId)
    {
        var project = new Project
        {
            Id = Guid.NewGuid(),
            Name = dto.Name,
            Description = dto.Description,
            OwnerId = userId,
            CreatedAt = DateTime.UtcNow
        };

        await _projectRepository.AddAsync(project);
        await _projectRepository.SaveChangesAsync();

        return _mapper.Map<ProjectDto>(project);
    }

    public async Task<ProjectDto?> UpdateAsync(UpdateProjectDto dto, Guid userId)
    {
        var project = await _projectRepository.GetByIdAsync(dto.Id);
        if (project == null || project.OwnerId != userId)
            return null;

        project.Name = dto.Name;
        project.Description = dto.Description;
        project.UpdatedAt = DateTime.UtcNow;

        await _projectRepository.UpdateAsync(project);
        await _projectRepository.SaveChangesAsync();

        return _mapper.Map<ProjectDto>(project);
    }

    public async Task<bool> DeleteAsync(Guid id, Guid userId)
    {
        var project = await _projectRepository.GetByIdAsync(id);
        if (project == null || project.OwnerId != userId)
            return false;

        await _projectRepository.DeleteAsync(id);
        await _projectRepository.SaveChangesAsync();
        return true;
    }
}