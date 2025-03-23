using EmployeeManagementSystem.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EmployeeManagementSystem.SeedConfiguration
{
    public class RoleConfiguration : IEntityTypeConfiguration<ApplicationRole>
    {
        public void Configure(EntityTypeBuilder<ApplicationRole> builder)
        {
            builder.HasData(
                new ApplicationRole
                {
                    Id = "835de03f-3984-4fff-96ec-37f8bd3bf180",
                    Name = "User",
                    NormalizedName = "USER"

                },
                new ApplicationRole
                {
                    Id = "550e8400-e29b-41d4-a716-446655440000",
                    Name = "Admin",
                    NormalizedName = "ADMIN"
                },
                new ApplicationRole
                {
                    Id = "8f14e45f-e1b3-45c9-bc1a-93e0e15f369f",
                    Name = "Employee",
                    NormalizedName = "EMPLOYEE"
                }
            );
        }
    }
}
