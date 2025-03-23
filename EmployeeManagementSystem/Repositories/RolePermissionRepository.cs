using EmployeeManagementSystem.Models;
using Microsoft.EntityFrameworkCore;
using System;

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

        public async Task<IEnumerable<Permission>> GetPermissionsByRoleIdAsync(string roleId)
        {
            return await _context.RolePermissions
                .Where(rp => rp.RoleId == roleId)
                .Select(rp => rp.Permission)
                .ToListAsync();
        }

        public async Task<bool> AssignPermissionToRoleAsync(RolePermission rolePermission)
        {
            var existingAssignment = await _context.RolePermissions
                .FirstOrDefaultAsync(rp => rp.RoleId == rolePermission.RoleId && rp.PermissionId == rolePermission.PermissionId);

            if (existingAssignment != null)
                return false; // Permission already assigned

            await _context.RolePermissions.AddAsync(rolePermission);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> UnassignPermissionFromRoleAsync(RolePermission rolePermission)
        {
            var existingAssignment = await _context.RolePermissions
                .FirstOrDefaultAsync(rp => rp.RoleId == rolePermission.RoleId && rp.PermissionId == rolePermission.PermissionId);

            if (existingAssignment == null)
                return false; // Permission not found

            _context.RolePermissions.Remove(existingAssignment);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<RolePermission> GetByIdAsync(string roleId, int permissionId)
        {
            return await _context.RolePermissions
                .FirstOrDefaultAsync(rp => rp.RoleId == roleId && rp.PermissionId == permissionId);
        }

        public async Task<bool> AddAsync(RolePermission rolePermission)
        {
            await _context.RolePermissions.AddAsync(rolePermission);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> DeleteAsync(string roleId, int permissionId)
        {
            var rolePermission = await _context.RolePermissions
                .FirstOrDefaultAsync(rp => rp.RoleId == roleId && rp.PermissionId == permissionId);

            if (rolePermission == null)
                return false; // RolePermission not found

            _context.RolePermissions.Remove(rolePermission);
            return await _context.SaveChangesAsync() > 0;
        }
    }
    
}
