using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace EmployeeManagementSystem.Models
{
    public class RolePermission
    {
        [ForeignKey("ApplicationRole")]
        public string RoleId { get; set; }
        [JsonIgnore]
        public ApplicationRole? Role { get; set; }

        [ForeignKey("Permission")]
        public int PermissionId { get; set; }
        [JsonIgnore]
        public Permission? Permission { get; set; }
    }
}
