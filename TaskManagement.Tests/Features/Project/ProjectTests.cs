using Moq;
using Xunit;
using FluentAssertions;
using TaskManagement.Application.DTOs.Project;
using TaskManagement.Application.Interfaces;
using TaskManagement.Application.Features.Project.Commands;
using TaskManagement.Application.Features.Project.Queries;

namespace TaskManagement.Tests.Features.Project;

public class GetAllProjectsQueryHandlerTests
{
    private readonly Mock<IProjectService> _mockProjectService;
    private readonly GetAllProjectsQueryHandler _handler;

    public GetAllProjectsQueryHandlerTests()
    {
        _mockProjectService = new Mock<IProjectService>();
        _handler = new GetAllProjectsQueryHandler(_mockProjectService.Object);
    }

    [Fact]
    public async System.Threading.Tasks.Task Handle_ReturnsProjectsList()
    {
        var userId = Guid.NewGuid();
        var projects = new System.Collections.Generic.List<ProjectDto>
        {
            new ProjectDto { Id = Guid.NewGuid(), Name = "Project 1" },
            new ProjectDto { Id = Guid.NewGuid(), Name = "Project 2" }
        };

        _mockProjectService.Setup(x => x.GetAllAsync(userId))
            .ReturnsAsync(projects);

        var query = new GetAllProjectsQuery(userId);
        var result = await _handler.Handle(query, CancellationToken.None);

        result.IsSuccess.Should().BeTrue();
        result.Value.Should().HaveCount(2);
    }
}

public class CreateProjectCommandHandlerTests
{
    private readonly Mock<IProjectService> _mockProjectService;
    private readonly CreateProjectCommandHandler _handler;

    public CreateProjectCommandHandlerTests()
    {
        _mockProjectService = new Mock<IProjectService>();
        _handler = new CreateProjectCommandHandler(_mockProjectService.Object);
    }

    [Fact]
    public async System.Threading.Tasks.Task Handle_ValidRequest_ReturnsProject()
    {
        var userId = Guid.NewGuid();
        var dto = new CreateProjectDto
        {
            Name = "New Project",
            Description = "Project description"
        };

        var createdProject = new ProjectDto
        {
            Id = Guid.NewGuid(),
            Name = dto.Name,
            Description = dto.Description
        };

        _mockProjectService.Setup(x => x.CreateAsync(dto, userId))
            .ReturnsAsync(createdProject);

        var command = new CreateProjectCommand(dto, userId);
        var result = await _handler.Handle(command, CancellationToken.None);

        result.IsSuccess.Should().BeTrue();
        result.Value.Should().NotBeNull();
        result.Value!.Name.Should().Be("New Project");
    }

    [Fact]
    public async System.Threading.Tasks.Task Handle_ServiceReturnsNull_ReturnsFailure()
    {
        var userId = Guid.NewGuid();
        var dto = new CreateProjectDto
        {
            Name = "New Project",
            Description = "Project description"
        };

        _mockProjectService.Setup(x => x.CreateAsync(dto, userId))
            .ReturnsAsync((ProjectDto?)null);

        var command = new CreateProjectCommand(dto, userId);
        var result = await _handler.Handle(command, CancellationToken.None);

        result.IsSuccess.Should().BeFalse();
    }
}