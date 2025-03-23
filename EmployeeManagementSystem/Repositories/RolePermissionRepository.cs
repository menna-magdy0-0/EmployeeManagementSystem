using EmployeeManagementSystem.Models;
using Microsoft.EntityFrameworkCore;

namespace EmployeeManagementSystem.Repositories
{
    public class RolePermissionRepository : IRolePermissionRepository
    {
        private readonly EmployeeContext _context;

        public RolePermissionRepository(EmployeeContext context)
        {
            _context = context;
        }

        // Get all RolePermission relationships
        public async Task<List<RolePermission>> GetAllAsync()
        {
            return await _context.RolePermissions
                .Include(rp => rp.Role)
                .Include(rp => rp.Permission)
                .ToListAsync();
        }

        // Get a specific RolePermission by RoleId and PermissionId
        public async Task<RolePermission?> GetByIdAsync(string roleId, int permissionId)
        {
            return await _context.RolePermissions
                .Include(rp => rp.Role)
                .Include(rp => rp.Permission)
                .FirstOrDefaultAsync(rp => rp.RoleId == roleId && rp.PermissionId == permissionId);
        }

        // Add a new RolePermission if it does not exist
        public async Task<bool> AddAsync(RolePermission rolePermission)
        {
            // Check if the relationship already exists
            var exists = await _context.RolePermissions
                .AnyAsync(rp => rp.RoleId == rolePermission.RoleId && rp.PermissionId == rolePermission.PermissionId);

            if (exists)
                return false;

            _context.RolePermissions.Add(rolePermission);
            await _context.SaveChangesAsync();
            return true;
        }

        // Delete a RolePermission by RoleId and PermissionId
        public async Task<bool> DeleteAsync(string roleId, int permissionId)
        {
            var rolePermission = await GetByIdAsync(roleId, permissionId);
            if (rolePermission == null) return false;

            _context.RolePermissions.Remove(rolePermission);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
