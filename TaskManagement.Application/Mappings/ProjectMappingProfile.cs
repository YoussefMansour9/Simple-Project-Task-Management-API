using AutoMapper;
using TaskManagement.Application.DTOs.Project;
using TaskManagement.Domain.Entities;

namespace TaskManagement.Application.Mappings;

public class ProjectMappingProfile : Profile
{
    public ProjectMappingProfile()
    {
        CreateMap<Project, ProjectDto>()
            .ForMember(dest => dest.TaskCount, opt => opt.MapFrom(src => src.Tasks.Count));
    }
}