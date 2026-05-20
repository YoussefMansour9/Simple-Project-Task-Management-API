using FluentValidation;
using TaskManagement.Domain.Enums;

namespace TaskManagement.Application.DTOs.Task;

public class UpdateTaskDto
{
    public Guid Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public Domain.Enums.TaskStatus Status { get; set; }
    public DateTime? DueDate { get; set; }
    public TaskPriority Priority { get; set; }
}

public class UpdateTaskValidator : AbstractValidator<UpdateTaskDto>
{
    public UpdateTaskValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty().WithMessage("Task ID is required");

        RuleFor(x => x.Title)
            .NotEmpty().WithMessage("Task title is required")
            .MaximumLength(200).WithMessage("Task title must not exceed 200 characters");

        RuleFor(x => x.Description)
            .MaximumLength(1000).WithMessage("Description must not exceed 1000 characters");

        RuleFor(x => x.DueDate)
            .Must(d => d == null || d > DateTime.UtcNow)
            .WithMessage("Due date must be in the future");
    }
}