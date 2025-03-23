using EmployeeManagementSystem.Models;
using Microsoft.EntityFrameworkCore;

namespace EmployeeManagementSystem.Repositories
{
    public class EmployeeRepository : IEmployeeRepository
    {
        private readonly EmployeeContext context;

        public EmployeeRepository(EmployeeContext _context)
        {
            context = _context;
        }
        public async Task<Employee> GetByIdAsync(int id)
        {
            return await context.Employees.FindAsync(id);
        }

        public async Task UpdateAsync(Employee employee)
        {
            context.Employees.Update(employee);
        }

        public async Task<bool> AddEmployeeAsync(Employee employee)
        {
            // Input Validation (Ensure Employee Code is not null)
            if (string.IsNullOrWhiteSpace(employee.EmployeeCode))
            {
                return false;
            }
            // Check for Unique Employee Code
            var existingEmployee = await context.Employees
                .AnyAsync(e => e.EmployeeCode == employee.EmployeeCode);

            //// Check if an employee already exists by unique identifiers (Email , phonenumber)
            //var existingEmployee = await context.Employees
            //    .AnyAsync(e => e.Email == employee.Email || e.PhoneNumber == employee.PhoneNumber);

            if (existingEmployee)
            {
                return false; // Prevent duplicate employees
            }

            //// Ensure a unique EmployeeCode
            //employee.EmployeeCode = await GenerateUniqueEmployeeCodeAsync();

            //employee.CreatedAt = DateTime.UtcNow;
            //employee.UpdatedAt = DateTime.UtcNow;

            await context.Employees.AddAsync(employee);
            await context.SaveChangesAsync();
            return true;
        }

        //private async Task<string> GenerateUniqueEmployeeCodeAsync()
        //{
        //    string employeeCode;

        //    // Keep generating until a unique code is found
        //    do
        //    {
        //        // Shortened GUID (8 characters for better readability)
        //        employeeCode = $"EMP-{Guid.NewGuid().ToString("N")[..8].ToUpper()}";
        //    }
        //    while (await context.Employees.AnyAsync(e => e.EmployeeCode == employeeCode));

        //    return employeeCode;
        //}

        public async Task<bool> DeleteEmployeeAsync(int id)
        {
            var employee = await GetEmployeeByIdAsync(id);
            if (employee == null)
            {
                return false;
            }
            // Prevent deletion if the employee is active
            if (employee.Status == EmployeeStatus.Active)
            {
                return false;
            }
            context.Employees.Remove(employee);
            await context.SaveChangesAsync();
            return true;
        }



        public async Task<List<Employee>> GetAllEmployeesAsync()
        {
            return await context.Employees.ToListAsync();
        }

        public async Task<List<Employee>> GetEmployeesAsync(string? name, string? jobTitle, decimal? minSalary, decimal? maxSalary, bool includeSalaryDetails = false)
        {
            var query = context.Employees.AsQueryable();

            // Apply Filtering
            if (!string.IsNullOrWhiteSpace(name))
            {
                query = query.Where(e => e.Name.Contains(name));
            }

            if (!string.IsNullOrWhiteSpace(jobTitle))
            {
                query = query.Where(e => e.JobTitle.Contains(jobTitle));
            }

            if (minSalary.HasValue)
            {
                query = query.Where(e => e.Salary >= minSalary.Value);
            }

            if (maxSalary.HasValue)
            {
                query = query.Where(e => e.Salary <= maxSalary.Value);
            }

            // Include Salary Details (Expand)
            if (includeSalaryDetails)
            {
                query = query.Include(e => e.Salary);
            }

            return await query.ToListAsync();
        }

        public async Task<Employee?> GetEmployeeByIdAsync(int id)
        {
            return await context.Employees.FirstOrDefaultAsync(e=>e.Id==id);
        }

        public async Task<Employee?> GetEmployeeByNameAsync(string name)
        {
            return await context.Employees.FirstOrDefaultAsync(e => e.Name == name);
        }

        public async Task<Employee?> GetEmployeeBySalaryAsync(decimal salary)
        {
            return await context.Employees.FirstOrDefaultAsync(e => e.Salary == salary);
        }

        public async Task<bool> UpdateEmployeeAsync(Employee employee)
        {
            var existingEmployee = await GetEmployeeByIdAsync(employee.Id);
            if (existingEmployee == null)
            {
                return false;
            }

            context.Entry(existingEmployee).CurrentValues.SetValues(employee);
            existingEmployee.UpdatedAt = DateTime.UtcNow;
            await context.SaveChangesAsync();
            return true;
        }
    }
}
