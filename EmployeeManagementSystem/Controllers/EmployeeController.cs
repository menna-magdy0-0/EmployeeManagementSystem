using EmployeeManagementSystem.Models;
using EmployeeManagementSystem.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EmployeeManagementSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeeController : ControllerBase
    {
        private readonly IEmployeeRepository empRepository;
        private readonly IUnitOfWork unitOfWork;

        public EmployeeController(IEmployeeRepository empRepository,IUnitOfWork unitOfWork)
        {
            this.empRepository = empRepository;
            this.unitOfWork = unitOfWork;
        }
        [HttpGet]
        public async Task<IActionResult> GetAllEmployees() 
        {
            var employees = await empRepository.GetAllEmployeesAsync();

            return Ok(employees);
        }
        [HttpGet("Filter")]
        public async Task<IActionResult> GetEmployees(
        [FromQuery] string? name,
        [FromQuery] string? jobTitle,
        [FromQuery] decimal? minSalary,
        [FromQuery] decimal? maxSalary,
        [FromQuery] bool expandSalary = false)
        {
            var employees = await unitOfWork.EmployeeRepository
                .GetEmployeesAsync(name, jobTitle, minSalary, maxSalary, expandSalary);

            if (employees == null || !employees.Any())
            {
                return NotFound("No employees found matching the criteria.");
            }

            return Ok(employees);
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetEmployeeById(int id)
        {
            var employee =await empRepository.GetEmployeeByIdAsync(id);
            if (employee == null)
            {
                return NotFound(new { Message = "Employee not found." });
            }
            return Ok(employee);
        }

        [HttpPost]
        public async Task<IActionResult> AddEmployee([FromBody]Employee employee)
        {
            if(!ModelState.IsValid) {
                return BadRequest(ModelState);
            }
            var isAdded=await empRepository.AddEmployeeAsync(employee);
            if (!isAdded)
            {
                return BadRequest(new { Message = "Employee not added." });
            }
            return CreatedAtAction(nameof(GetEmployeeById), new { id = employee.Id }, employee);
            
        }
        [HttpPut("{id:int}")]
        public async Task<IActionResult> UpdateEmployee(int id, [FromBody] Employee employee)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            // Retrieve existing employee
            var existingEmployee = await unitOfWork.EmployeeRepository.GetByIdAsync(id);
            if (existingEmployee == null)
                return NotFound("Employee not found.");

            // Ensure Employee Code remains unchanged
            if (existingEmployee.EmployeeCode != employee.EmployeeCode)
                return BadRequest("Employee Code cannot be modified.");

            // Update allowed properties
            existingEmployee.Name = employee.Name;
            existingEmployee.Email = employee.Email;
            existingEmployee.Address= employee.Address;

            // Save changes
            await unitOfWork.EmployeeRepository.UpdateAsync(existingEmployee);
            await unitOfWork.CompleteAsync();

            return Ok("Employee updated successfully.");
            //if (id != employee.Id)
            //{
            //    return BadRequest(new { Message = "Employee ID mismatch." });
            //}

            //var isUpdated = await empRepository.UpdateEmployeeAsync(employee);
            //if (!isUpdated)
            //{
            //    return BadRequest(new { Message = "Employee not found or could not be updated." });
            //}
            //return Ok(new { Message = "Employee updated successfully." });
        }
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> DeleteEmployee(int id)
        {
            var isDeleted = await empRepository.DeleteEmployeeAsync(id);
            if (!isDeleted)
            {
                return BadRequest(new { Message = "Cannot delete active employee or employee not found." });
            }
            return Ok(new { Message = "Employee deleted successfully." });
        }
        
    }
}
