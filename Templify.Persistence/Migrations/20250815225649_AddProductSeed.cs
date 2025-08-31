using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Templify.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddProductSeed : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Products",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Description = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: false),
                    ImageUrl = table.Column<string>(type: "character varying(300)", maxLength: 300, nullable: false),
                    Price = table.Column<decimal>(type: "numeric", nullable: false),
                    Author = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Category = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Downloads = table.Column<int>(type: "integer", nullable: false),
                    Tags = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    CreatedBy = table.Column<int>(type: "integer", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    UpdatedBy = table.Column<int>(type: "integer", nullable: true),
                    UpdatedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Products", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "Products",
                columns: new[] { "Id", "Author", "Category", "CreatedBy", "CreatedDate", "Description", "Downloads", "ImageUrl", "Name", "Price", "Tags", "UpdatedBy", "UpdatedDate" },
                values: new object[,]
                {
                    { 1, "DesignVista", "Automotive", null, null, "Power Automotive AI", 4580, "~/src/img/tem1.png", "Craftify", 99m, "ai, automotive, power", null, null },
                    { 2, "DesignCraft", "Cards", null, null, "Modern Card Platform", 3580, "~/src/img/tem2.png", "PixelPro", 99m, "cards, modern, ui", null, null },
                    { 3, "CodeMaven", "Education", null, null, "Design Education", 2980, "~/src/img/tem3.png", "DesignHub", 99m, "education, design, hub", null, null },
                    { 4, "WebMaster", "Health", null, null, "Mental Health Platform", 2504, "~/src/img/tem4.png", "TemplateForge", 99m, "health, mental, platform", null, null }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Products");
        }
    }
}
