using EmployeeManagementSystem.DTOs;
using EmployeeManagementSystem.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace EmployeeManagementSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Policy = "OnlyAdminUsers")]
    public class UserRoleController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<ApplicationRole> _roleManager;

        public UserRoleController(UserManager<ApplicationUser> userManager, RoleManager<ApplicationRole> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }

        [HttpPost("assign")] // POST: api/UserRole/assign
        public async Task<IActionResult> AssignRole([FromBody] UserRoleDto request)
        {
            var user = await _userManager.FindByIdAsync(request.UserId);
            if (user == null)
                return NotFound("User not found.");

            if (!await _roleManager.RoleExistsAsync(request.RoleName))
                return BadRequest("Role does not exist.");

            if (await _userManager.IsInRoleAsync(user, request.RoleName))
                return BadRequest("User already has this role.");

            var result = await _userManager.AddToRoleAsync(user, request.RoleName);
            if (!result.Succeeded)
                return BadRequest(result.Errors);

            return Ok("Role assigned successfully.");
        }

        [HttpPost("remove")] // POST: api/UserRole/remove
        public async Task<IActionResult> RemoveRole([FromBody] UserRoleDto request)
        {
            var user = await _userManager.FindByIdAsync(request.UserId);
            if (user == null)
                return NotFound("User not found.");

            if (!await _userManager.IsInRoleAsync(user, request.RoleName))
                return BadRequest("User does not have this role.");

            var result = await _userManager.RemoveFromRoleAsync(user, request.RoleName);
            if (!result.Succeeded)
                return BadRequest(result.Errors);

            return Ok("Role removed successfully.");
        }

        [HttpGet("user/{userId}")] // GET: api/UserRole/user/{userId}
        public async Task<IActionResult> GetUserRoles(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
                return NotFound("User not found.");

            var roles = await _userManager.GetRolesAsync(user);
            return Ok(roles);
        }
    }
}
