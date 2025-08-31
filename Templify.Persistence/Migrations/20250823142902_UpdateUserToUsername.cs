using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Templify.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class UpdateUserToUsername : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TemplateSubs_AppUsers_AppUserId",
                table: "TemplateSubs");

            migrationBuilder.DropForeignKey(
                name: "FK_TemplateSubs_Products_ProductId",
                table: "TemplateSubs");

            migrationBuilder.DropIndex(
                name: "IX_TemplateSubs_AppUserId_ProductId",
                table: "TemplateSubs");

            migrationBuilder.DropIndex(
                name: "IX_TemplateSubs_Created",
                table: "TemplateSubs");

            migrationBuilder.DropColumn(
                name: "FirstName",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "LastName",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "FirstName",
                table: "AppUsers");

            migrationBuilder.DropColumn(
                name: "LastName",
                table: "AppUsers");

            migrationBuilder.RenameColumn(
                name: "UserName",
                table: "AspNetUsers",
                newName: "Username");

            migrationBuilder.AlterColumn<string>(
                name: "Username",
                table: "AspNetUsers",
                type: "text",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "character varying(256)",
                oldMaxLength: 256,
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UserName",
                table: "AspNetUsers",
                type: "character varying(256)",
                maxLength: 256,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Username",
                table: "AppUsers",
                type: "character varying(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_TemplateSubs_AppUserId",
                table: "TemplateSubs",
                column: "AppUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_TemplateSubs_AppUsers_AppUserId",
                table: "TemplateSubs",
                column: "AppUserId",
                principalTable: "AppUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_TemplateSubs_Products_ProductId",
                table: "TemplateSubs",
                column: "ProductId",
                principalTable: "Products",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TemplateSubs_AppUsers_AppUserId",
                table: "TemplateSubs");

            migrationBuilder.DropForeignKey(
                name: "FK_TemplateSubs_Products_ProductId",
                table: "TemplateSubs");

            migrationBuilder.DropIndex(
                name: "IX_TemplateSubs_AppUserId",
                table: "TemplateSubs");

            migrationBuilder.DropColumn(
                name: "UserName",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "Username",
                table: "AppUsers");

            migrationBuilder.RenameColumn(
                name: "Username",
                table: "AspNetUsers",
                newName: "UserName");

            migrationBuilder.AlterColumn<string>(
                name: "UserName",
                table: "AspNetUsers",
                type: "character varying(256)",
                maxLength: 256,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AddColumn<string>(
                name: "FirstName",
                table: "AspNetUsers",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "LastName",
                table: "AspNetUsers",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "FirstName",
                table: "AppUsers",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "LastName",
                table: "AppUsers",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_TemplateSubs_AppUserId_ProductId",
                table: "TemplateSubs",
                columns: new[] { "AppUserId", "ProductId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_TemplateSubs_Created",
                table: "TemplateSubs",
                column: "Created");

            migrationBuilder.AddForeignKey(
                name: "FK_TemplateSubs_AppUsers_AppUserId",
                table: "TemplateSubs",
                column: "AppUserId",
                principalTable: "AppUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_TemplateSubs_Products_ProductId",
                table: "TemplateSubs",
                column: "ProductId",
                principalTable: "Products",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
