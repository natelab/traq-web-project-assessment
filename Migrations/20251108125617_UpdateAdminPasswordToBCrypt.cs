using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace traq_web_project_assessment.Migrations
{
    /// <inheritdoc />
    public partial class UpdateAdminPasswordToBCrypt : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                column: "PasswordHash",
                value: "$2a$12$LQKFoLvJZfYBYf.YPnJGauYnlZHYQQhWQhYPzPvPU8Sf/xpPqYZsO");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                column: "PasswordHash",
                value: "AQAAAAEAACcQAAAAEBvZx1YHd8K9L0YxLYyYxYzQ8w==");
        }
    }
}
