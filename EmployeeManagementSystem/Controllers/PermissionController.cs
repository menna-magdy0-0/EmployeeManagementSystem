using EmployeeManagementSystem.Models;
using EmployeeManagementSystem.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Data;

namespace EmployeeManagementSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PermissionController : ControllerBase
    {
        private readonly IPermissionRepository _permissionRepository;
        private readonly IRolePermissionRepository _rolePermissionRepository;
        

        public PermissionController(IPermissionRepository permissionRepository, IRolePermissionRepository rolePermissionRepository)
        {
            _permissionRepository = permissionRepository;
            _rolePermissionRepository = rolePermissionRepository;
            
        }

        // GET: api/permission
        [HttpGet]
        public async Task<IActionResult> GetAllPermissions()
        {
            var permissions = await _permissionRepository.GetAllPermissionsAsync();
            return Ok(permissions);
        }

        // GET: api/permission/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> GetPermissionById(int id)
        {
            var permission = await _permissionRepository.GetPermissionByIdAsync(id);
            if (permission == null) return NotFound("Permission not found.");
            return Ok(permission);
        }

        // GET: api/permission/role/{roleId}
        [HttpGet("role/{roleId}")]
        public async Task<IActionResult> GetPermissionsByRole(string roleId)
        {
            var permissions = await _rolePermissionRepository.GetPermissionsByRoleIdAsync(roleId);
            return Ok(permissions);
        }

        // POST: api/permission
        [HttpPost]
        [Authorize(Policy = "ManagePermissions")]
        public async Task<IActionResult> CreatePermission([FromBody] Permission permission)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            bool isAdded = await _permissionRepository.AddPermissionAsync(permission);
            if (!isAdded) return Conflict("Permission with this name already exists.");

            return CreatedAtAction(nameof(GetPermissionById), new { id = permission.Id }, permission);
        }

        // POST: api/permission/assign
        [HttpPost("assign")]
        [Authorize(Policy = "ManagePermissions")]
        public async Task<IActionResult> AssignPermissionToRole([FromBody] RolePermission rolePermission)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            bool isAssigned = await _rolePermissionRepository.AssignPermissionToRoleAsync(rolePermission);
            if (!isAssigned) return Conflict("Permission is already assigned to the role.");

            return Ok("Permission assigned to role successfully.");
        }

        // DELETE: api/permission/unassign
        [HttpDelete("unassign")]
        [Authorize(Policy = "ManagePermissions")]
        public async Task<IActionResult> UnassignPermissionFromRole([FromBody] RolePermission rolePermission)
        {
            bool isUnassigned = await _rolePermissionRepository.UnassignPermissionFromRoleAsync(rolePermission);
            if (!isUnassigned) return NotFound("Permission not found for the specified role.");

            return Ok("Permission unassigned from role successfully.");
        }

        // PUT: api/permission/{id}
        [HttpPut("{id}")]
        [Authorize(Policy = "ManagePermissions")]
        public async Task<IActionResult> UpdatePermission(int id, [FromBody] Permission permission)
        {
            if (id != permission.Id) return BadRequest("Mismatched Permission ID.");
            if (!ModelState.IsValid) return BadRequest(ModelState);

            bool isUpdated = await _permissionRepository.UpdatePermissionAsync(permission);
            if (!isUpdated) return NotFound("Permission not found.");

            return NoContent();
        }

        // DELETE: api/permission/{id}
        [HttpDelete("{id}")]
        [Authorize(Policy = "ManagePermissions")]
        public async Task<IActionResult> DeletePermission(int id)
        {
            bool isDeleted = await _permissionRepository.DeletePermissionAsync(id);
            if (!isDeleted) return NotFound("Permission not found.");

            return NoContent();
        }

        
    }
}
