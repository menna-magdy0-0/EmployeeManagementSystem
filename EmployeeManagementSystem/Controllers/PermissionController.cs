using EmployeeManagementSystem.Models;
using EmployeeManagementSystem.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EmployeeManagementSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PermissionController : ControllerBase
    {
        private readonly IPermissionRepository _permissionRepository;

        public PermissionController(IPermissionRepository permissionRepository)
        {
            _permissionRepository = permissionRepository;
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

        // POST: api/permission
        [HttpPost]
        public async Task<IActionResult> CreatePermission([FromBody] Permission permission)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            bool isAdded = await _permissionRepository.AddPermissionAsync(permission);
            if (!isAdded) return Conflict("Permission with this name already exists.");

            return CreatedAtAction(nameof(GetPermissionById), new { id = permission.Id }, permission);
        }

        // PUT: api/permission/{id}
        [HttpPut("{id}")]
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
        public async Task<IActionResult> DeletePermission(int id)
        {
            bool isDeleted = await _permissionRepository.DeletePermissionAsync(id);
            if (!isDeleted) return NotFound("Permission not found.");

            return NoContent();
        }
    }
}
