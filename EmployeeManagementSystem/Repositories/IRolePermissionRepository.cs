using EmployeeManagementSystem.Models;

namespace EmployeeManagementSystem.Repositories
{
    public interface IRolePermissionRepository
    {
        Task<List<RolePermission>> GetAllAsync();
        Task<IEnumerable<Permission>> GetPermissionsByRoleIdAsync(string roleId);
        Task<bool> AssignPermissionToRoleAsync(RolePermission rolePermission);
        Task<bool> UnassignPermissionFromRoleAsync(RolePermission rolePermission);
        Task<RolePermission> GetByIdAsync(string roleId, int permissionId);
        Task<bool> AddAsync(RolePermission rolePermission);
        Task<bool> DeleteAsync(string roleId, int permissionId); // Add this method to the interface
    }
    
}
