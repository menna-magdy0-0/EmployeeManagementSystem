using Microsoft.AspNetCore.Identity;
using System.Text.Json.Serialization;

namespace EmployeeManagementSystem.Models
{
    public class ApplicationRole:IdentityRole
    {
        //public int Id { get; set; }
        public string Name { get; set; }

        // Navigation property for users
        [JsonIgnore]
        public ICollection<ApplicationUser>? Users { get; set; } = new List<ApplicationUser>();
        [JsonIgnore]
        public ICollection<RolePermission>? RolePermissions { get; set; } = new List<RolePermission>();
        //public ICollection<Permission> Permissions { get; set; } = new List<Permission>();

        //[JsonIgnore]
        //public List<Permission>? Permissions { get; set; }


    }
}
