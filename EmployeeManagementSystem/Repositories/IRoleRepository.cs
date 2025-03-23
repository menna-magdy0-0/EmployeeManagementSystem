using EmployeeManagementSystem.Models;

namespace EmployeeManagementSystem.Repositories
{
    public interface IRoleRepository
    {
        Task<List<ApplicationRole>> GetAllRolesAsync();
        Task<ApplicationRole?> GetRoleByIdAsync(string id);
        Task<bool> AddRoleAsync(ApplicationRole role);
        Task<bool> UpdateRoleAsync(ApplicationRole role);
        Task<bool> DeleteRoleAsync(string id);
    }
}
