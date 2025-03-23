using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System.Reflection.Emit;
using System.Text;

namespace EmployeeManagementSystem.Models
{
    public class EmployeeContext : IdentityDbContext<ApplicationUser,ApplicationRole,string>
    {
        public EmployeeContext(DbContextOptions<EmployeeContext> options) : base(options)
        {
        }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.Entity<RolePermission>()
                .HasKey(rp => new { rp.RoleId, rp.PermissionId });

            //Define one-to - many relationship
            builder.Entity<ApplicationUser>()
                .HasOne(u => u.Role)
                .WithMany(r => r.Users)
                .HasForeignKey(u => u.ApplicationRoleId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<RolePermission>()
                .HasOne(rp => rp.Role)
                .WithMany(r => r.RolePermissions)
                .HasForeignKey(rp => rp.RoleId)
                .HasPrincipalKey(r => r.Id); // Ensure RoleId maps to IdentityRole's Id

            builder.Entity<RolePermission>()
                .HasOne(rp => rp.Permission)
                .WithMany(p => p.RolePermissions)
                .HasForeignKey(rp => rp.PermissionId);
        }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            var modifiedEntities=ChangeTracker.Entries()
                .Where(e=>e.State == EntityState.Modified
                ||e.State == EntityState.Deleted
                ||e.State==EntityState.Added) .ToList();
            foreach (var modifiedEntity in modifiedEntities)
            {
                var auditLog = new AuditLog
                {
                    EntityType=modifiedEntity.Entity.GetType().Name,
                    ActionType=modifiedEntity.State.ToString(),
                    TimeStamp=DateTime.UtcNow,
                    Changes=GetChanges(modifiedEntity)
                };
                AuditLogs.Add(auditLog);
            }
            return base.SaveChangesAsync(cancellationToken);
        }
        private static string GetChanges(EntityEntry entity)
        {
            var changes = new StringBuilder();
            foreach (var property in entity.OriginalValues.Properties)
            {
                var originalValue = entity.OriginalValues[property];
                var currentValue = entity.CurrentValues[property];
                if (!Equals(originalValue, currentValue))
                {
                    changes.AppendLine($"{property.Name}: From '{originalValue}' to '{currentValue}'");
                }
            }
            return changes.ToString();
        }
        public DbSet<Employee> Employees { get; set; }
        public DbSet<Permission> Permissions { get; set; }
        public DbSet<ApplicationRole> ApplicationRoles { get; set; }
        public DbSet<ApplicationUser> ApplicationUsers { get; set; }
        public DbSet<AuditLog> AuditLogs { get; set; }
        public DbSet<RolePermission> RolePermissions { get; set; }
    }
}
