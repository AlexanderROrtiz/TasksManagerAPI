using TaskManagement.Domain.Entities;

namespace TaskManagement.Domain.Interfaces
{
    public interface IRoleRepository
    {
        Task<Role> GetByIdAsync(int roleId);
        Task<Role> GetByNameAsync(string roleName);
        Task AddRoleAsync(Role role);
    }
}
