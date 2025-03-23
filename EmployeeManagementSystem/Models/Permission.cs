using System.Text.Json.Serialization;

namespace EmployeeManagementSystem.Models
{
    public class Permission
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string? Description { get; set; }
        
        
        [JsonIgnore]
        public ICollection<RolePermission>? RolePermissions { get; set; } = new List<RolePermission>();
        //public List<ApplicationRole>? Roles { get; set; }
    }
}
