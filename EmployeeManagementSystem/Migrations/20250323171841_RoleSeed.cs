using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace EmployeeManagementSystem.Migrations
{
    /// <inheritdoc />
    public partial class RoleSeed : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "550e8400-e29b-41d4-a716-446655440000", null, "Admin", "ADMIN" },
                    { "835de03f-3984-4fff-96ec-37f8bd3bf180", null, "User", "USER" },
                    { "8f14e45f - e1b3 - 45c9 - bc1a - 93e0e15f369f", null, "Employee", "EMPLOYEE" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "550e8400-e29b-41d4-a716-446655440000");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "835de03f-3984-4fff-96ec-37f8bd3bf180");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "8f14e45f - e1b3 - 45c9 - bc1a - 93e0e15f369f");
        }
    }
}
