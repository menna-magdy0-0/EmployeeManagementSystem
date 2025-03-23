using EmployeeManagementSystem.Models;

namespace EmployeeManagementSystem.Repositories
{
    public interface IRolePermissionRepository
    {
        Task<List<RolePermission>> GetAllAsync();
        Task<RolePermission?> GetByIdAsync(string roleId, int permissionId);
        Task<bool> AddAsync(RolePermission rolePermission);
        Task<bool> DeleteAsync(string roleId, int permissionId);
    }
}
