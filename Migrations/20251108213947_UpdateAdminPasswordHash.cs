using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace traq_web_project_assessment.Migrations
{
    /// <inheritdoc />
    public partial class UpdateAdminPasswordHash : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                column: "PasswordHash",
                value: "$2a$12$84mSCPey6C.XPpLm15ROPuPiuTZs1YbOTUiM.N2BWko8MS/r39QC2");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                column: "PasswordHash",
                value: "$2a$12$LQKFoLvJZfYBYf.YPnJGauYnlZHYQQhWQhYPzPvPU8Sf/xpPqYZsO");
        }
    }
}
