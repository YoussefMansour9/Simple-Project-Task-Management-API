namespace TaskManagement.Application.DTOs.Task;

public class UpdateTaskStatusDto
{
    public Guid Id { get; set; }
    public string Status { get; set; } = string.Empty;
}