using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace EmployeeManagementSystem.Models
{
    public class AuditLog
    {
        public int Id { get; set; }
        public string ActionType { get; set; } //Create, Update, Delete
        public string EntityType { get; set; }//Afficted by Employee , User, Role, Permission
        public string Changes { get; set; }
        public DateTime TimeStamp { get; set; } = DateTime.UtcNow;
        [ForeignKey("ApplicationUser")]
        public int UserId { get; set; }
        [JsonIgnore]
        public ApplicationUser? User { get; set; }

    }
}
