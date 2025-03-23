using EmployeeManagementSystem.Models;
using Microsoft.EntityFrameworkCore;

namespace EmployeeManagementSystem.Repositories
{
    public class RoleRepository : IRoleRepository
    {
        private readonly EmployeeContext _context;

        public RoleRepository(EmployeeContext context)
        {
            _context = context;
        }

        // Get All Roles
        public async Task<List<ApplicationRole>> GetAllRolesAsync()
        {
            return await _context.ApplicationRoles
                .Include(r => r.RolePermissions) // Include RolePermissions for completeness
                .Include(r => r.Users) // Include Users associated with the Role
                .ToListAsync();
        }

        // Get Role by ID
        public async Task<ApplicationRole?> GetRoleByIdAsync(string id)
        {
            return await _context.ApplicationRoles
                .Include(r => r.RolePermissions)
                .Include(r => r.Users)
                .FirstOrDefaultAsync(r => r.Id == id);
        }

        // Add a New Role (with duplicate check)
        public async Task<bool> AddRoleAsync(ApplicationRole role)
        {
            var existingRole = await _context.ApplicationRoles
                .AnyAsync(r => r.Name == role.Name);
            if (existingRole)
            {
                return false; // Role already exists
            }

            await _context.ApplicationRoles.AddAsync(role);
            await _context.SaveChangesAsync();
            return true;
        }

        // Update Existing Role
        public async Task<bool> UpdateRoleAsync(ApplicationRole role)
        {
            var existingRole = await GetRoleByIdAsync(role.Id);
            if (existingRole == null) return false; // Role not found

            existingRole.Name = role.Name;
            //existingRole.Description = role.Description;

            _context.ApplicationRoles.Update(existingRole);
            await _context.SaveChangesAsync();
            return true;
        }

        // Delete Role (with cascade check)
        public async Task<bool> DeleteRoleAsync(string id)
        {
            var role = await GetRoleByIdAsync(id);
            if (role == null) return false; // Role not found

            _context.ApplicationRoles.Remove(role);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
