using Microsoft.EntityFrameworkCore;
using TaskManagement.Application.DTOs.TasksDTOs;
using TaskManagement.Application.Interfaces;
using TaskManagement.Domain.Entities;

namespace TaskManagement.Application.Services
{
    public class TaskService : ITaskService
    {
        private readonly ITaskRepository _taskRepository;

        public TaskService(ITaskRepository taskRepository)
        {
            _taskRepository = taskRepository;
        }

        public async Task<TaskDto> CreateTaskAsync(TaskDto taskDto)
        {
            var task = new Taskss
            {
                Title = taskDto.Title,
                Description = taskDto.Description,
                Status = "Pendiente",
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now
            };

            await _taskRepository.AddTaskAsync(task);

            return new TaskDto
            {
                Id = task.Id,
                Title = task.Title,
                Description = task.Description,
                Status = task.Status,
                CreatedAt = task.CreatedAt,
                UpdatedAt = task.UpdatedAt
            };
        }

        public async Task<TaskDto> GetTaskByIdAsync(int id)
        {
            var task = await _taskRepository.GetTaskByIdAsync(id);
            if (task == null) return null;

            return new TaskDto
            {
                Id = task.Id,
                Title = task.Title,
                Description = task.Description,
                Status = task.Status,
                CreatedAt = task.CreatedAt,
                UpdatedAt = task.UpdatedAt,
                AssignedToUserId = task.AssignedToUserId,
                AssignedToUsername = task.AssignedToUser?.Username 
            };
        }
        public async Task<List<TaskDto>> GetAllTasksAsync()
        {
            var tasks = await _taskRepository.GetAllTasksAsync();

            return tasks.Select(task => new TaskDto
            {
                Id = task.Id,
                Title = task.Title,
                Description = task.Description,
                Status = task.Status,
                CreatedAt = task.CreatedAt,
                UpdatedAt = task.UpdatedAt,
                AssignedToUserId = task.AssignedToUserId,
                AssignedToUsername = task.AssignedToUser?.Username
            }).ToList();
        }

        public async Task<List<TaskDto>> GetTasksByUserIdAsync(int userId)
        {
            var tasks = await _taskRepository.GetTasksByUserIdAsync(userId);

            return tasks.Select(task => new TaskDto
            {
                Id = task.Id,
                Title = task.Title,
                Description = task.Description,
                Status = task.Status,
                CreatedAt = task.CreatedAt,
                UpdatedAt = task.UpdatedAt,
                AssignedToUserId = task.AssignedToUserId,
                AssignedToUsername = task.AssignedToUser?.Username
            }).ToList();
        }

        public async Task<bool> AssignTaskAsync(int taskId, AssignTaskDto assignTaskDto)
        {
            var task = await _taskRepository.GetTaskByIdAsync(taskId);
            if (task == null) return false;

            task.AssignedToUserId = assignTaskDto.UserId;
            task.UpdatedAt = DateTime.Now;

            await _taskRepository.UpdateTaskAsync(task);
            return true;
        }

        public async Task<bool> UpdateTaskStatusAsync(int taskId, UpdateTaskStatusDto updateTaskStatusDto, int userId)
        {
            var task = await _taskRepository.GetTaskByIdAsync(taskId);
            if (task == null || task.AssignedToUserId != userId) return false;

            task.Status = updateTaskStatusDto.Status;
            task.UpdatedAt = DateTime.Now;

            await _taskRepository.UpdateTaskAsync(task);
            return true;
        }

        public async Task<bool> DeleteTaskAsync(int id)
        {
            var task = await _taskRepository.GetTaskByIdAsync(id);
            if (task == null) return false;

            await _taskRepository.DeleteTaskAsync(id);
            return true;
        }

        public async Task<bool> AssignTaskAsync(int taskId, int userId)
        {
            var task = await _taskRepository.GetTaskByIdAsync(taskId);
            if (task == null) return false;
            await _taskRepository.UpdateTaskAsync(task);
            return true;
        }

        public async Task<bool> UpdateTaskStatusAsync(int taskId, string status)
        {
            var task = await _taskRepository.GetTaskByIdAsync(taskId);
            if (task == null) return false;
            await _taskRepository.UpdateTaskAsync(task);
            return true;
        }
    }
}
