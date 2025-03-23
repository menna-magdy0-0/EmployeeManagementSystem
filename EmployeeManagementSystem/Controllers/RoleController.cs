using EmployeeManagementSystem.Models;
using EmployeeManagementSystem.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EmployeeManagementSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RoleController : ControllerBase
    {
        private readonly IRoleRepository _roleRepository;

        public RoleController(IRoleRepository roleRepository)
        {
            _roleRepository = roleRepository;
        }

        // GET: api/role
        [HttpGet]
        public async Task<IActionResult> GetAllRoles()
        {
            var roles = await _roleRepository.GetAllRolesAsync();
            return Ok(roles);
        }

        // GET: api/role/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> GetRoleById(string id)
        {
            var role = await _roleRepository.GetRoleByIdAsync(id);
            if (role == null) return NotFound("Role not found.");
            return Ok(role);
        }

        // POST: api/role
        [HttpPost]
        public async Task<IActionResult> CreateRole([FromBody] ApplicationRole role)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            bool isAdded = await _roleRepository.AddRoleAsync(role);
            if (!isAdded) return Conflict("Role name already exists.");

            return CreatedAtAction(nameof(GetRoleById), new { id = role.Id }, role);
        }

        // PUT: api/role/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateRole(string id, [FromBody] ApplicationRole role)
        {
            if (id != role.Id) return BadRequest("Mismatched Role ID.");
            if (!ModelState.IsValid) return BadRequest(ModelState);

            bool isUpdated = await _roleRepository.UpdateRoleAsync(role);
            if (!isUpdated) return NotFound("Role not found.");

            return NoContent();
        }

        // DELETE: api/role/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteRole(string id)
        {
            bool isDeleted = await _roleRepository.DeleteRoleAsync(id);
            if (!isDeleted) return NotFound("Role not found.");

            return NoContent();
        }
    }
}
