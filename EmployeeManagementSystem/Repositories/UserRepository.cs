using EmployeeManagementSystem.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace EmployeeManagementSystem.Repositories
{
    public class UserRepository: IUserRepository
    {
        private readonly EmployeeContext _context;

        public UserRepository(EmployeeContext context)
        {
            _context = context;
        }

        // Get All Users
        public async Task<List<ApplicationUser>> GetAllUsersAsync()
        {
            return await _context.ApplicationUsers
                                .Include(u => u.Role) // Eager loading to include the related role
                                .ToListAsync();
        }

        // Get User by ID
        public async Task<ApplicationUser?> GetUserByIdAsync(string id)
        {
            return await _context.ApplicationUsers
                                 .Include(u => u.Role)
                                 .FirstOrDefaultAsync(u => u.Id == id);
        }

        // Get User by Username (for login or validation)
        public async Task<ApplicationUser?> GetUserByUserNameAsync(string userName)
        {
            return await _context.ApplicationUsers
                                 .Include(u => u.Role)
                                 .FirstOrDefaultAsync(u => u.UserName == userName);
        }

        // Add a New User (with duplicate check)
        public async Task<bool> AddUserAsync(ApplicationUser user)
        {
            var existingUser = await _context.ApplicationUsers.AnyAsync(u => u.UserName == user.UserName);
            if (existingUser)
            {
                return false; // User already exists
            }
            //// Assign default role if not provided
            //if (user.ApplicationRoleId == 0)
            //{
            //    var defaultRole = await _context.ApplicationRoles
            //        .FirstOrDefaultAsync(r => r.Name == "User");

            //    if (defaultRole == null)
            //        throw new InvalidOperationException("Default role not found.");

            //    user.ApplicationRoleId = defaultRole.Id;
            //}

            await _context.ApplicationUsers.AddAsync(user);
            await _context.SaveChangesAsync();
            return true;
        }

        // Update Existing User
        public async Task<bool> UpdateUserAsync(ApplicationUser user)
        {
            var existingUser = await GetUserByIdAsync(user.Id);
            if (existingUser == null) return false; // Not found

            // Update properties
            existingUser.UserName = user.UserName;
            existingUser.PasswordHash = user.PasswordHash;
            existingUser.ApplicationRoleId = user.ApplicationRoleId;

            _context.ApplicationUsers.Update(existingUser);
            await _context.SaveChangesAsync();
            return true;
        }

        // Delete User (if exists)
        public async Task<bool> DeleteUserAsync(string id)
        {
            var user = await GetUserByIdAsync(id);
            if (user == null) return false; // Not found

            _context.ApplicationUsers.Remove(user);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
