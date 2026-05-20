using Moq;
using Xunit;
using FluentAssertions;
using TaskManagement.Application.DTOs.Task;
using TaskManagement.Application.Interfaces;
using TaskManagement.Application.Features.Task.Commands;
using TaskManagement.Application.Features.Task.Queries;

namespace TaskManagement.Tests.Features.Task;

public class GetTasksByProjectQueryHandlerTests
{
    private readonly Mock<ITaskService> _mockTaskService;
    private readonly GetTasksByProjectQueryHandler _handler;

    public GetTasksByProjectQueryHandlerTests()
    {
        _mockTaskService = new Mock<ITaskService>();
        _handler = new GetTasksByProjectQueryHandler(_mockTaskService.Object);
    }

    [Fact]
    public async System.Threading.Tasks.Task Handle_ReturnsTasksList()
    {
        var projectId = Guid.NewGuid();
        var userId = Guid.NewGuid();
        var tasks = new System.Collections.Generic.List<TaskDto>
        {
            new TaskDto { Id = Guid.NewGuid(), Title = "Task 1" },
            new TaskDto { Id = Guid.NewGuid(), Title = "Task 2" }
        };

        _mockTaskService.Setup(x => x.GetByProjectIdAsync(projectId, userId))
            .ReturnsAsync(tasks);

        var query = new GetTasksByProjectQuery(projectId, userId);
        var result = await _handler.Handle(query, CancellationToken.None);

        result.IsSuccess.Should().BeTrue();
        result.Value.Should().HaveCount(2);
    }
}

public class CreateTaskCommandHandlerTests
{
    private readonly Mock<ITaskService> _mockTaskService;
    private readonly CreateTaskCommandHandler _handler;

    public CreateTaskCommandHandlerTests()
    {
        _mockTaskService = new Mock<ITaskService>();
        _handler = new CreateTaskCommandHandler(_mockTaskService.Object);
    }

    [Fact]
    public async System.Threading.Tasks.Task Handle_ValidRequest_ReturnsTask()
    {
        var userId = Guid.NewGuid();
        var dto = new CreateTaskDto
        {
            Title = "New Task",
            Description = "Task description",
            ProjectId = Guid.NewGuid()
        };

        var createdTask = new TaskDto
        {
            Id = Guid.NewGuid(),
            Title = dto.Title,
            Description = dto.Description
        };

        _mockTaskService.Setup(x => x.CreateAsync(dto, userId))
            .ReturnsAsync(createdTask);

        var command = new CreateTaskCommand(dto, userId);
        var result = await _handler.Handle(command, CancellationToken.None);

        result.IsSuccess.Should().BeTrue();
        result.Value.Should().NotBeNull();
        result.Value!.Title.Should().Be("New Task");
    }
}

public class UpdateTaskStatusCommandHandlerTests
{
    private readonly Mock<ITaskService> _mockTaskService;
    private readonly UpdateTaskStatusCommandHandler _handler;

    public UpdateTaskStatusCommandHandlerTests()
    {
        _mockTaskService = new Mock<ITaskService>();
        _handler = new UpdateTaskStatusCommandHandler(_mockTaskService.Object);
    }

    [Fact]
    public async System.Threading.Tasks.Task Handle_ValidStatus_ReturnsUpdatedTask()
    {
        var taskId = Guid.NewGuid();
        var userId = Guid.NewGuid();
        var updatedTask = new TaskDto
        {
            Id = taskId,
            Title = "Task",
            Status = "InProgress"
        };

        _mockTaskService.Setup(x => x.UpdateStatusAsync(taskId, "InProgress", userId))
            .ReturnsAsync(updatedTask);

        var command = new UpdateTaskStatusCommand(taskId, "InProgress", userId);
        var result = await _handler.Handle(command, CancellationToken.None);

        result.IsSuccess.Should().BeTrue();
        result.Value!.Status.Should().Be("InProgress");
    }
}