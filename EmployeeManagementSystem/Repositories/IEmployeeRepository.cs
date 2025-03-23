using EmployeeManagementSystem.Models;

namespace EmployeeManagementSystem.Repositories
{
    public interface IEmployeeRepository
    {
        Task<Employee> GetByIdAsync(int id);
        Task UpdateAsync(Employee employee);

        Task<List<Employee>> GetEmployeesAsync(string? name, string? jobTitle, decimal? minSalary, decimal? maxSalary, bool includeSalaryDetails = false);

        Task<bool> AddEmployeeAsync(Employee employee);

        Task<Employee?> GetEmployeeByIdAsync(int id);
        Task<Employee?> GetEmployeeByNameAsync(string name);
        Task<Employee?> GetEmployeeBySalaryAsync(decimal salary);
        Task<List<Employee>> GetAllEmployeesAsync();

        Task<bool> UpdateEmployeeAsync(Employee employee);

        Task<bool> DeleteEmployeeAsync(int id);

    }
}
