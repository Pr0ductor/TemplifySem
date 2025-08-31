using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Templify.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddMoreProductSeed : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Products",
                columns: new[] { "Id", "Author", "Category", "CreatedBy", "CreatedDate", "Description", "Downloads", "ImageUrl", "Name", "Price", "Tags", "UpdatedBy", "UpdatedDate" },
                values: new object[,]
                {
                    { 5, "ThemeWiz", "App", null, null, "Drinking App", 1805, "~/src/img/tem5.png", "SiteSwift", 99m, "app, drinking, swift", null, null },
                    { 6, "PixelGenius", "AI", null, null, "AI Search Engine", 1506, "~/src/img/tem6.png", "WebAura", 99m, "ai, search, engine", null, null },
                    { 7, "WebMajestic", "Finance", null, null, "Finance Platform", 1200, "~/src/img/tem7.png", "Setora", 99m, "finance, platform", null, null },
                    { 8, "TemplatePro", "Banking", null, null, "Banking Platform", 1100, "~/src/img/tem8.png", "WebAura", 99m, "banking, platform", null, null },
                    { 9, "PixelGenius", "Feedback", null, null, "Customer Feedback", 900, "~/src/img/tem9.png", "PixelGame", 99m, "feedback, customer", null, null },
                    { 10, "DesignVista", "Brand", null, null, "Brand Performance", 800, "~/src/img/tem10.png", "Letona", 99m, "brand, performance", null, null },
                    { 11, "TemplatePro", "Intelligence", null, null, "Intelligence Platform", 700, "~/src/img/tem11.png", "Growthmedia", 99m, "intelligence, platform", null, null },
                    { 12, "WebStudio", "AI", null, null, "No-Code AI Tool", 600, "~/src/img/tem12.png", "Craftify", 99m, "ai, no-code, tool", null, null }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 7);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 8);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 9);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 10);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 11);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 12);
        }
    }
}
