using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Templify.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class UpdateAuthorForUserIntegration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "Authors",
                type: "boolean",
                nullable: false,
                defaultValue: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsSeedAuthor",
                table: "Authors",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "UserId",
                table: "Authors",
                type: "character varying(450)",
                maxLength: 450,
                nullable: true);

            migrationBuilder.UpdateData(
                table: "Authors",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "IsActive", "IsSeedAuthor", "UserId" },
                values: new object[] { true, true, null });

            migrationBuilder.UpdateData(
                table: "Authors",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "IsActive", "IsSeedAuthor", "UserId" },
                values: new object[] { true, true, null });

            migrationBuilder.UpdateData(
                table: "Authors",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "IsActive", "IsSeedAuthor", "UserId" },
                values: new object[] { true, true, null });

            migrationBuilder.UpdateData(
                table: "Authors",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "IsActive", "IsSeedAuthor", "UserId" },
                values: new object[] { true, true, null });

            migrationBuilder.UpdateData(
                table: "Authors",
                keyColumn: "Id",
                keyValue: 5,
                columns: new[] { "IsActive", "IsSeedAuthor", "UserId" },
                values: new object[] { true, true, null });

            migrationBuilder.UpdateData(
                table: "Authors",
                keyColumn: "Id",
                keyValue: 6,
                columns: new[] { "IsActive", "IsSeedAuthor", "UserId" },
                values: new object[] { true, true, null });

            migrationBuilder.UpdateData(
                table: "Authors",
                keyColumn: "Id",
                keyValue: 7,
                columns: new[] { "IsActive", "IsSeedAuthor", "UserId" },
                values: new object[] { true, true, null });

            migrationBuilder.UpdateData(
                table: "Authors",
                keyColumn: "Id",
                keyValue: 8,
                columns: new[] { "IsActive", "IsSeedAuthor", "UserId" },
                values: new object[] { true, true, null });

            migrationBuilder.UpdateData(
                table: "Authors",
                keyColumn: "Id",
                keyValue: 9,
                columns: new[] { "IsActive", "IsSeedAuthor", "UserId" },
                values: new object[] { true, true, null });

            migrationBuilder.CreateIndex(
                name: "IX_Authors_IsActive",
                table: "Authors",
                column: "IsActive");

            migrationBuilder.CreateIndex(
                name: "IX_Authors_IsSeedAuthor",
                table: "Authors",
                column: "IsSeedAuthor");

            migrationBuilder.CreateIndex(
                name: "IX_Authors_UserId",
                table: "Authors",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Authors_AspNetUsers_UserId",
                table: "Authors",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Authors_AspNetUsers_UserId",
                table: "Authors");

            migrationBuilder.DropIndex(
                name: "IX_Authors_IsActive",
                table: "Authors");

            migrationBuilder.DropIndex(
                name: "IX_Authors_IsSeedAuthor",
                table: "Authors");

            migrationBuilder.DropIndex(
                name: "IX_Authors_UserId",
                table: "Authors");

            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "Authors");

            migrationBuilder.DropColumn(
                name: "IsSeedAuthor",
                table: "Authors");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "Authors");
        }
    }
}
