using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Templify.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddCategoryTypeToProduct : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CategoryType",
                table: "Products",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "Category", "CategoryType", "Description", "Name", "Tags" },
                values: new object[] { "Business", 1, "Professional Business Platform", "BusinessPro", "business, professional, platform" });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "Category", "CategoryType", "Description", "Name", "Price", "Tags" },
                values: new object[] { "Business", 1, "Corporate Management System", "CorporateHub", 89m, "corporate, management, system" });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "Category", "CategoryType", "Description", "Name", "Price", "Tags" },
                values: new object[] { "3D Web", 2, "3D Web Design Platform", "3DWebStudio", 129m, "3d, web, design" });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "Category", "CategoryType", "Description", "Name", "Price", "Tags" },
                values: new object[] { "3D Web", 2, "3D Interactive Experience", "ThreeDimensional", 149m, "3d, interactive, experience" });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 5,
                columns: new[] { "Category", "CategoryType", "Description", "Name", "Price", "Tags" },
                values: new object[] { "Saas Platforms", 3, "Cloud-based SaaS Platform", "CloudSaaS", 199m, "saas, cloud, platform" });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 6,
                columns: new[] { "Category", "CategoryType", "Description", "Name", "Price", "Tags" },
                values: new object[] { "Saas Platforms", 3, "Professional SaaS Solution", "SaaSPro", 179m, "saas, professional, solution" });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 7,
                columns: new[] { "Category", "CategoryType", "Description", "Name", "Price", "Tags" },
                values: new object[] { "Agency", 4, "Digital Agency Platform", "AgencyHub", 159m, "agency, digital, platform" });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 8,
                columns: new[] { "Category", "CategoryType", "Description", "Name", "Price", "Tags" },
                values: new object[] { "Agency", 4, "Creative Agency Website", "CreativeAgency", 139m, "creative, agency, website" });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 9,
                columns: new[] { "Category", "CategoryType", "Description", "Name", "Price", "Tags" },
                values: new object[] { "Portfolio Design", 5, "Professional Portfolio", "PortfolioPro", 79m, "portfolio, professional, design" });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 10,
                columns: new[] { "Category", "CategoryType", "Description", "Name", "Price", "Tags" },
                values: new object[] { "Portfolio Design", 5, "Creative Design Portfolio", "DesignPortfolio", 69m, "design, portfolio, creative" });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 11,
                columns: new[] { "Category", "CategoryType", "Description", "Name", "Price", "Tags" },
                values: new object[] { "Ecommerce", 6, "E-commerce Store Platform", "EcomStore", 189m, "ecommerce, store, platform" });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 12,
                columns: new[] { "Category", "CategoryType", "Description", "Name", "Price", "Tags" },
                values: new object[] { "Ecommerce", 6, "Professional Shopping Site", "ShopPro", 169m, "shop, professional, ecommerce" });

            migrationBuilder.InsertData(
                table: "Products",
                columns: new[] { "Id", "Author", "Category", "CategoryType", "CreatedBy", "CreatedDate", "Description", "Downloads", "ImageUrl", "Name", "Price", "Tags", "UpdatedBy", "UpdatedDate" },
                values: new object[,]
                {
                    { 13, "DesignVista", "Education", 7, null, null, "Educational Platform", 550, "~/src/img/tem1.png", "EduPlatform", 119m, "education, platform, learning", null, null },
                    { 14, "DesignCraft", "Education", 7, null, null, "Learning Management System", 480, "~/src/img/tem2.png", "LearnHub", 109m, "learning, management, system", null, null },
                    { 15, "CodeMaven", "Health", 8, null, null, "Healthcare Platform", 420, "~/src/img/tem3.png", "HealthCare", 159m, "health, care, platform", null, null },
                    { 16, "WebMaster", "Health", 8, null, null, "Medical Services Website", 380, "~/src/img/tem4.png", "MedicalHub", 149m, "medical, services, health", null, null },
                    { 17, "ThemeWiz", "Marketing", 9, null, null, "Marketing Agency Platform", 350, "~/src/img/tem5.png", "MarketingPro", 139m, "marketing, agency, platform", null, null },
                    { 18, "PixelGenius", "Marketing", 9, null, null, "Advertising Campaign Site", 320, "~/src/img/tem6.png", "AdCampaign", 129m, "advertising, campaign, marketing", null, null },
                    { 19, "WebMajestic", "Restaurant & Food", 10, null, null, "Restaurant Management Platform", 290, "~/src/img/tem7.png", "RestaurantHub", 119m, "restaurant, food, management", null, null },
                    { 20, "TemplatePro", "Restaurant & Food", 10, null, null, "Food Delivery Service", 260, "~/src/img/tem8.png", "FoodDelivery", 109m, "food, delivery, service", null, null },
                    { 21, "PixelGenius", "Gaming & Entertainment", 11, null, null, "Gaming Studio Platform", 240, "~/src/img/tem9.png", "GameStudio", 169m, "gaming, studio, platform", null, null },
                    { 22, "DesignVista", "Gaming & Entertainment", 11, null, null, "Entertainment Website", 220, "~/src/img/tem10.png", "EntertainmentHub", 159m, "entertainment, gaming, website", null, null },
                    { 23, "TemplatePro", "Real Estate", 12, null, null, "Real Estate Platform", 200, "~/src/img/tem11.png", "RealEstatePro", 179m, "real estate, property, platform", null, null },
                    { 24, "WebStudio", "Real Estate", 12, null, null, "Property Management System", 180, "~/src/img/tem12.png", "PropertyHub", 169m, "property, management, real estate", null, null }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 13);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 14);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 15);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 16);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 17);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 18);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 19);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 20);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 21);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 22);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 23);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 24);

            migrationBuilder.DropColumn(
                name: "CategoryType",
                table: "Products");

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "Category", "Description", "Name", "Tags" },
                values: new object[] { "Automotive", "Power Automotive AI", "Craftify", "ai, automotive, power" });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "Category", "Description", "Name", "Price", "Tags" },
                values: new object[] { "Cards", "Modern Card Platform", "PixelPro", 99m, "cards, modern, ui" });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "Category", "Description", "Name", "Price", "Tags" },
                values: new object[] { "Education", "Design Education", "DesignHub", 99m, "education, design, hub" });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "Category", "Description", "Name", "Price", "Tags" },
                values: new object[] { "Health", "Mental Health Platform", "TemplateForge", 99m, "health, mental, platform" });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 5,
                columns: new[] { "Category", "Description", "Name", "Price", "Tags" },
                values: new object[] { "App", "Drinking App", "SiteSwift", 99m, "app, drinking, swift" });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 6,
                columns: new[] { "Category", "Description", "Name", "Price", "Tags" },
                values: new object[] { "AI", "AI Search Engine", "WebAura", 99m, "ai, search, engine" });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 7,
                columns: new[] { "Category", "Description", "Name", "Price", "Tags" },
                values: new object[] { "Finance", "Finance Platform", "Setora", 99m, "finance, platform" });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 8,
                columns: new[] { "Category", "Description", "Name", "Price", "Tags" },
                values: new object[] { "Banking", "Banking Platform", "WebAura", 99m, "banking, platform" });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 9,
                columns: new[] { "Category", "Description", "Name", "Price", "Tags" },
                values: new object[] { "Feedback", "Customer Feedback", "PixelGame", 99m, "feedback, customer" });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 10,
                columns: new[] { "Category", "Description", "Name", "Price", "Tags" },
                values: new object[] { "Brand", "Brand Performance", "Letona", 99m, "brand, performance" });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 11,
                columns: new[] { "Category", "Description", "Name", "Price", "Tags" },
                values: new object[] { "Intelligence", "Intelligence Platform", "Growthmedia", 99m, "intelligence, platform" });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 12,
                columns: new[] { "Category", "Description", "Name", "Price", "Tags" },
                values: new object[] { "AI", "No-Code AI Tool", "Craftify", 99m, "ai, no-code, tool" });
        }
    }
}
