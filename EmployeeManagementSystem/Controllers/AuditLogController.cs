using EmployeeManagementSystem.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EmployeeManagementSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuditLogController : ControllerBase
    {
        private readonly IAuditLogRepository _repository;

        public AuditLogController(IAuditLogRepository repository)
        {
            _repository = repository;
        }

        // GET: api/AuditLog
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var logs = await _repository.GetAllAsync();
            return Ok(logs);
        }
    }
}
