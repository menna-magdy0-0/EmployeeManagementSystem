using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace EmployeeManagementSystem.Models
{
    public class User
    {
        public int Id { get; set; }
        public string UserName { get; set; }
        public string PasswordHash { get; set; }

        [ForeignKey("ApplicationRole")]
        public int ApplicationRoleId { get; set; }
        public ApplicationRole? Role { get; set; }
        [JsonIgnore]
        public List<AuditLog>? AuditLogs { get; set; }//changes made by this user


    }
}
