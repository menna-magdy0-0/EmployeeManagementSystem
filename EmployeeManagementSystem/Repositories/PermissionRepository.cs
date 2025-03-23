using EmployeeManagementSystem.Models;
using Microsoft.EntityFrameworkCore;

namespace EmployeeManagementSystem.Repositories
{
    public class PermissionRepository : IPermissionRepository
    {
        private readonly EmployeeContext _context;

        public PermissionRepository(EmployeeContext context)
        {
            _context = context;
        }

        // Get All Permissions
        public async Task<List<Permission>> GetAllPermissionsAsync()
        {
            return await _context.Permissions
                .Include(p => p.RolePermissions) // Include RolePermissions for better traceability
                .ToListAsync();
        }

        // Get Permission by ID
        public async Task<Permission?> GetPermissionByIdAsync(int id)
        {
            return await _context.Permissions
                .Include(p => p.RolePermissions) // Include RolePermissions
                .FirstOrDefaultAsync(p => p.Id == id);
        }

        // Add a New Permission (with duplicate check)
        public async Task<bool> AddPermissionAsync(Permission permission)
        {
            var exists = await _context.Permissions
                .AnyAsync(p => p.Name == permission.Name);
            if (exists) return false; // Avoid duplicate permissions

            await _context.Permissions.AddAsync(permission);
            await _context.SaveChangesAsync();
            return true;
        }

        // Update Existing Permission
        public async Task<bool> UpdatePermissionAsync(Permission permission)
        {
            var existingPermission = await GetPermissionByIdAsync(permission.Id);
            if (existingPermission == null) return false; // Permission not found

            existingPermission.Name = permission.Name;
            existingPermission.Description = permission.Description;

            _context.Permissions.Update(existingPermission);
            await _context.SaveChangesAsync();
            return true;
        }

        // Delete Permission (with relationship check)
        public async Task<bool> DeletePermissionAsync(int id)
        {
            var permission = await GetPermissionByIdAsync(id);
            if (permission == null) return false; // Permission not found

            _context.Permissions.Remove(permission);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
