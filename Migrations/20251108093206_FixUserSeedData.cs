using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace traq_web_project_assessment.Migrations
{
    /// <inheritdoc />
    public partial class FixUserSeedData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedDate",
                value: new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedDate",
                value: new DateTime(2025, 11, 8, 11, 13, 41, 746, DateTimeKind.Local).AddTicks(5024));
        }
    }
}
