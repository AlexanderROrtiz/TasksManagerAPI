using Microsoft.EntityFrameworkCore;
using TaskManagement.Application.Interfaces;
using TaskManagement.Domain.Entities;
using TaskManagement.Infrastructure.Data;

namespace TaskManagement.Infrastructure.Repositories
{
    public class TaskRepository : ITaskRepository
    {
        private readonly AppDbContext _context;

        public TaskRepository(AppDbContext context)
        {
            _context = context;
        }
        public async Task<List<Taskss>> GetTasksByUserIdAsync(int userId)
        {
            return await _context.Taskss
                .Where(t => t.AssignedToUserId == userId)
                .ToListAsync();
        }

        public async Task<Taskss> GetTaskByIdAsync(int id)
        {
            return await _context.Taskss.FindAsync(id);
        }

        public async Task AddTaskAsync(Taskss task)
        {
            await _context.Taskss.AddAsync(task);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateTaskAsync(Taskss task)
        {
            _context.Taskss.Update(task);
            await _context.SaveChangesAsync();
        }
        public async Task<List<Taskss>> GetAllTasksAsync()
        {
            return await _context.Taskss.ToListAsync();
        }

        public async Task DeleteTaskAsync(int id)
        {
            var task = await _context.Taskss.FindAsync(id);
            if (task != null)
            {
                _context.Taskss.Remove(task);
                await _context.SaveChangesAsync();
            }
        }
    }

}
