using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace EmployeeManagementSystem.Models
{
    public class ApplicationUser:IdentityUser
    {
        //public new string UserName { get; set; }

        // Foreign key
        [ForeignKey("ApplicationRole")]
        public string ApplicationRoleId { get; set; }

        // Navigation property
        [JsonIgnore]
        public ApplicationRole? Role { get; set; }

        //public int Id { get; set; }
        //public string UserName { get; set; }
        //public string PasswordHash { get; set; }

        //[ForeignKey("Employee")]
        //public int EmployeeId { get; set; }
        //public Employee? Employee { get; set; }

        //[ForeignKey("Role")]
        //public int RoleId { get; set; }
        //public Role? Role { get; set; }
        //[JsonIgnore]
        //public List<AuditLog>? AuditLogs { get; set; }//changes made by this user
    }
}
