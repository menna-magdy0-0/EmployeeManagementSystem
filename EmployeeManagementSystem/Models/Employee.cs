using System.ComponentModel.DataAnnotations;

namespace EmployeeManagementSystem.Models
{
    public class Employee
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Employee Code is required.")]
        [RegularExpression(@"^[A-Za-z0-9]{6,10}$",
        ErrorMessage = "Employee Code must be 6-10 characters long and contain only letters and numbers.")]
        public string EmployeeCode { get; set; }//unique employee Identifier 
        [Required(ErrorMessage = "Name is required.")]
        public string Name { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string Address { get; set; }
        public DateOnly HireDate { get; set; }
        public string JobTitle { get; set; }
        public decimal Salary { get; set; }
        public EmployeeStatus Status { get; set; } = EmployeeStatus.Active;// Active, Inactive, Terminated

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

    }

    public enum EmployeeStatus
    {
        Active,     // Currently working
        Inactive,   // On leave or temporarily not working
        Terminated  // No longer employed
    }

}
