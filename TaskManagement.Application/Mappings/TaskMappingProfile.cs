using AutoMapper;
using TaskManagement.Application.DTOs.Task;
using TaskManagement.Domain.Entities;

namespace TaskManagement.Application.Mappings;

public class TaskMappingProfile : Profile
{
    public TaskMappingProfile()
    {
        CreateMap<ProjectTask, TaskDto>()
            .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status.ToString()))
            .ForMember(dest => dest.Priority, opt => opt.MapFrom(src => src.Priority.ToString()))
            .ForMember(dest => dest.ProjectName, opt => opt.MapFrom(src => src.Project.Name));
    }
}