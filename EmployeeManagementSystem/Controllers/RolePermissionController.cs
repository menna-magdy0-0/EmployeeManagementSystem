using EmployeeManagementSystem.Models;
using EmployeeManagementSystem.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EmployeeManagementSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RolePermissionController : ControllerBase
    {
        private readonly IRolePermissionRepository _repository;

        public RolePermissionController(IRolePermissionRepository repository)
        {
            _repository = repository;
        }

        // GET: api/rolepermission
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var rolePermissions = await _repository.GetAllAsync();
            return Ok(rolePermissions);
        }

        // GET: api/rolepermission/{roleId}/{permissionId}
        [HttpGet("{roleId}/{permissionId}")]
        public async Task<IActionResult> GetById(string roleId, int permissionId)
        {
            var rolePermission = await _repository.GetByIdAsync(roleId, permissionId);
            if (rolePermission == null) return NotFound("Role-Permission relationship not found.");

            return Ok(rolePermission);
        }

        // POST: api/rolepermission
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] RolePermission rolePermission)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            bool added = await _repository.AddAsync(rolePermission);
            if (!added) return Conflict("This Role-Permission relationship already exists.");

            return CreatedAtAction(nameof(GetById), new { roleId = rolePermission.RoleId, permissionId = rolePermission.PermissionId }, rolePermission);
        }

        // DELETE: api/rolepermission/{roleId}/{permissionId}
        [HttpDelete("{roleId}/{permissionId}")]
        public async Task<IActionResult> Delete(string roleId, int permissionId)
        {
            bool deleted = await _repository.DeleteAsync(roleId, permissionId);
            if (!deleted) return NotFound("Role-Permission relationship not found.");

            return NoContent();
        }
    }
}
