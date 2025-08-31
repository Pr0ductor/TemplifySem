using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Templify.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddAuthorRole : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "CreatedAt", "Description", "IsActive", "Name", "NormalizedName" },
                values: new object[] { "3", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Автор шаблонов", true, "Author", "AUTHOR" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "3");
        }
    }
}
