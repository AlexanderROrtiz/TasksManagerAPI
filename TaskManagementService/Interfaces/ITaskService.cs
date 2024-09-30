using TaskManagement.Application.DTOs.TasksDTOs;

namespace TaskManagement.Application.Interfaces
{
    public interface ITaskService
    {
        Task<TaskDto> CreateTaskAsync(TaskDto taskDto);
        Task<List<TaskDto>> GetAllTasksAsync();
        Task<TaskDto> GetTaskByIdAsync(int id);
        Task<List<TaskDto>> GetTasksByUserIdAsync(int userId);
        Task<bool> AssignTaskAsync(int taskId, AssignTaskDto assignTaskDto);
        Task<bool> UpdateTaskStatusAsync(int taskId, UpdateTaskStatusDto updateTaskStatusDto, int userId);
        Task<bool> DeleteTaskAsync(int id);
    }
}
