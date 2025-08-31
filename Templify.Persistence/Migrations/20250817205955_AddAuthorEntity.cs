using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Templify.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddAuthorEntity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "AuthorId",
                table: "Products",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "Authors",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Bio = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: false),
                    AvatarUrl = table.Column<string>(type: "character varying(300)", maxLength: 300, nullable: false),
                    Email = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Website = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    SocialLinks = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false),
                    TotalProducts = table.Column<int>(type: "integer", nullable: false),
                    TotalDownloads = table.Column<int>(type: "integer", nullable: false),
                    Rating = table.Column<double>(type: "double precision", nullable: false),
                    Specialization = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    CreatedBy = table.Column<int>(type: "integer", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    UpdatedBy = table.Column<int>(type: "integer", nullable: true),
                    UpdatedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Authors", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "Authors",
                columns: new[] { "Id", "AvatarUrl", "Bio", "CreatedBy", "CreatedDate", "Email", "Name", "Rating", "SocialLinks", "Specialization", "TotalDownloads", "TotalProducts", "UpdatedBy", "UpdatedDate", "Website" },
                values: new object[,]
                {
                    { 1, "~/src/img/person1.jpg", "Award-winning design studio specializing in modern web templates and digital experiences. With over 10 years of experience, we create stunning, user-friendly designs that help businesses stand out in the digital landscape.", null, null, "hello@designvista.com", "DesignVista", 4.7999999999999998, "{\"twitter\":\"@designvista\",\"linkedin\":\"designvista\",\"dribbble\":\"designvista\"}", "Web Design & UI/UX", 15800, 8, null, null, "https://designvista.com" },
                    { 2, "~/src/img/person2.jpg", "Creative agency focused on delivering exceptional digital solutions. Our team of passionate designers and developers work together to create templates that combine beauty with functionality.", null, null, "contact@designcraft.com", "DesignCraft", 4.7000000000000002, "{\"twitter\":\"@designcraft\",\"linkedin\":\"designcraft\",\"behance\":\"designcraft\"}", "Creative Design & Branding", 12400, 6, null, null, "https://designcraft.com" },
                    { 3, "~/src/img/person3.jpg", "Full-stack development studio with expertise in modern web technologies. We specialize in creating robust, scalable templates that developers love to work with.", null, null, "hello@codemaven.com", "CodeMaven", 4.9000000000000004, "{\"twitter\":\"@codemaven\",\"github\":\"codemaven\",\"linkedin\":\"codemaven\"}", "Web Development & 3D", 9800, 5, null, null, "https://codemaven.com" },
                    { 4, "~/src/img/person4.jpg", "Experienced web developer and designer with a passion for creating intuitive user experiences. Specializing in responsive design and modern web standards.", null, null, "contact@webmaster.com", "WebMaster", 4.5999999999999996, "{\"twitter\":\"@webmaster\",\"linkedin\":\"webmaster\",\"codepen\":\"webmaster\"}", "Frontend Development", 7200, 4, null, null, "https://webmaster.com" },
                    { 5, "~/src/img/person5.jpg", "Premium theme development studio creating high-quality templates for businesses worldwide. Our focus is on performance, accessibility, and user experience.", null, null, "hello@themewiz.com", "ThemeWiz", 4.7999999999999998, "{\"twitter\":\"@themewiz\",\"linkedin\":\"themewiz\",\"dribbble\":\"themewiz\"}", "Premium Themes & SaaS", 13500, 7, null, null, "https://themewiz.com" },
                    { 6, "~/src/img/person6.jpg", "Creative design studio pushing the boundaries of digital design. We create unique, innovative templates that inspire and engage users across all platforms.", null, null, "contact@pixelgenius.com", "PixelGenius", 4.9000000000000004, "{\"twitter\":\"@pixelgenius\",\"behance\":\"pixelgenius\",\"instagram\":\"pixelgenius\"}", "Creative Design & Animation", 16800, 9, null, null, "https://pixelgenius.com" },
                    { 7, "~/src/img/person1.jpg", "Professional web development agency specializing in custom solutions and premium templates. We help businesses establish a strong online presence.", null, null, "hello@webmajestic.com", "WebMajestic", 4.7000000000000002, "{\"twitter\":\"@webmajestic\",\"linkedin\":\"webmajestic\",\"github\":\"webmajestic\"}", "Web Development & Agency", 11200, 6, null, null, "https://webmajestic.com" },
                    { 8, "~/src/img/person2.jpg", "Leading template marketplace with a focus on quality and innovation. Our team creates templates that help businesses grow and succeed online.", null, null, "contact@templatepro.com", "TemplatePro", 4.7999999999999998, "{\"twitter\":\"@templatepro\",\"linkedin\":\"templatepro\",\"dribbble\":\"templatepro\"}", "Template Development", 22400, 12, null, null, "https://templatepro.com" },
                    { 9, "~/src/img/person3.jpg", "Modern web studio creating cutting-edge digital experiences. We combine creativity with technology to deliver exceptional results for our clients.", null, null, "hello@webstudio.com", "WebStudio", 4.5999999999999996, "{\"twitter\":\"@webstudio\",\"linkedin\":\"webstudio\",\"behance\":\"webstudio\"}", "Modern Web Development", 8900, 5, null, null, "https://webstudio.com" }
                });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 1,
                column: "AuthorId",
                value: 1);

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 2,
                column: "AuthorId",
                value: 2);

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 3,
                column: "AuthorId",
                value: 3);

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 4,
                column: "AuthorId",
                value: 4);

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 5,
                column: "AuthorId",
                value: 5);

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 6,
                column: "AuthorId",
                value: 6);

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 7,
                column: "AuthorId",
                value: 7);

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 8,
                column: "AuthorId",
                value: 8);

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 9,
                column: "AuthorId",
                value: 6);

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 10,
                column: "AuthorId",
                value: 1);

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 11,
                column: "AuthorId",
                value: 8);

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 12,
                column: "AuthorId",
                value: 9);

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 13,
                column: "AuthorId",
                value: 1);

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 14,
                column: "AuthorId",
                value: 2);

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 15,
                column: "AuthorId",
                value: 3);

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 16,
                column: "AuthorId",
                value: 4);

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 17,
                column: "AuthorId",
                value: 5);

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 18,
                column: "AuthorId",
                value: 6);

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 19,
                column: "AuthorId",
                value: 7);

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 20,
                column: "AuthorId",
                value: 8);

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 21,
                column: "AuthorId",
                value: 6);

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 22,
                column: "AuthorId",
                value: 1);

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 23,
                column: "AuthorId",
                value: 8);

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 24,
                column: "AuthorId",
                value: 9);

            migrationBuilder.CreateIndex(
                name: "IX_Products_AuthorId",
                table: "Products",
                column: "AuthorId");

            migrationBuilder.AddForeignKey(
                name: "FK_Products_Authors_AuthorId",
                table: "Products",
                column: "AuthorId",
                principalTable: "Authors",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Products_Authors_AuthorId",
                table: "Products");

            migrationBuilder.DropTable(
                name: "Authors");

            migrationBuilder.DropIndex(
                name: "IX_Products_AuthorId",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "AuthorId",
                table: "Products");
        }
    }
}
