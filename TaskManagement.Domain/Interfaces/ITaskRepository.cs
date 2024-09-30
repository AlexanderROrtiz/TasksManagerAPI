using TaskManagement.Domain.Entities;

namespace TaskManagement.Application.Interfaces
{
    public interface ITaskRepository
    {
        Task<List<Taskss>> GetTasksByUserIdAsync(int userId);
        Task<Taskss> GetTaskByIdAsync(int id);
        Task<List<Taskss>> GetAllTasksAsync();
        Task AddTaskAsync(Taskss task);
        Task UpdateTaskAsync(Taskss task);
        Task DeleteTaskAsync(int id);    
    }
}
