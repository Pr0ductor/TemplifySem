using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Templify.Domain.Entities;

namespace Templify.Persistence.Configurations;

public class AuthorConfiguration : IEntityTypeConfiguration<Author>
{
    public void Configure(EntityTypeBuilder<Author> builder)
    {
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Name).IsRequired().HasMaxLength(100);
        builder.Property(x => x.Bio).HasMaxLength(1000);
        builder.Property(x => x.AvatarUrl).HasMaxLength(300);
        builder.Property(x => x.Email).HasMaxLength(100);
        builder.Property(x => x.Website).HasMaxLength(200);
        builder.Property(x => x.SocialLinks).HasMaxLength(500);
        builder.Property(x => x.Specialization).HasMaxLength(100);
        
        // New fields for user integration
        builder.Property(x => x.UserId).HasMaxLength(450); // Standard length for Identity user IDs
        builder.Property(x => x.IsSeedAuthor).IsRequired().HasDefaultValue(false);
        builder.Property(x => x.IsActive).IsRequired().HasDefaultValue(true);
        
        // Relationship with ApplicationUser
        builder.HasOne(x => x.User)
               .WithMany()
               .HasForeignKey(x => x.UserId)
               .OnDelete(DeleteBehavior.SetNull); // Don't delete author if user is deleted
        
        // Index for performance
        builder.HasIndex(x => x.UserId);
        builder.HasIndex(x => x.IsActive);
        builder.HasIndex(x => x.IsSeedAuthor);

        builder.HasData(
            new Author
            {
                Id = 1,
                Name = "DesignVista",
                Bio = "Award-winning design studio specializing in modern web templates and digital experiences. With over 10 years of experience, we create stunning, user-friendly designs that help businesses stand out in the digital landscape.",
                AvatarUrl = "~/src/img/person1.jpg",
                Email = "hello@designvista.com",
                Website = "https://designvista.com",
                SocialLinks = "{\"twitter\":\"@designvista\",\"linkedin\":\"designvista\",\"dribbble\":\"designvista\"}",
                TotalProducts = 8,
                TotalDownloads = 15800,
                Specialization = "Web Design & UI/UX",
                UserId = null,
                IsSeedAuthor = true,
                IsActive = true,
            },
            new Author
            {
                Id = 2,
                Name = "DesignCraft",
                Bio = "Creative agency focused on delivering exceptional digital solutions. Our team of passionate designers and developers work together to create templates that combine beauty with functionality.",
                AvatarUrl = "~/src/img/person2.jpg",
                Email = "contact@designcraft.com",
                Website = "https://designcraft.com",
                SocialLinks = "{\"twitter\":\"@designcraft\",\"linkedin\":\"designcraft\",\"behance\":\"designcraft\"}",
                TotalProducts = 6,
                TotalDownloads = 12400,
                Specialization = "Creative Design & Branding",
                UserId = null,
                IsSeedAuthor = true,
                IsActive = true,
            },
            new Author
            {
                Id = 3,
                Name = "CodeMaven",
                Bio = "Full-stack development studio with expertise in modern web technologies. We specialize in creating robust, scalable templates that developers love to work with.",
                AvatarUrl = "~/src/img/person3.jpg",
                Email = "hello@codemaven.com",
                Website = "https://codemaven.com",
                SocialLinks = "{\"twitter\":\"@codemaven\",\"github\":\"codemaven\",\"linkedin\":\"codemaven\"}",
                TotalProducts = 5,
                TotalDownloads = 9800,
                Specialization = "Web Development & 3D",
                UserId = null,
                IsSeedAuthor = true,
                IsActive = true,
            },
            new Author
            {
                Id = 4,
                Name = "WebMaster",
                Bio = "Experienced web developer and designer with a passion for creating intuitive user experiences. Specializing in responsive design and modern web standards.",
                AvatarUrl = "~/src/img/person4.jpg",
                Email = "contact@webmaster.com",
                Website = "https://webmaster.com",
                SocialLinks = "{\"twitter\":\"@webmaster\",\"linkedin\":\"webmaster\",\"codepen\":\"webmaster\"}",
                TotalProducts = 4,
                TotalDownloads = 7200,
                Specialization = "Frontend Development",
                UserId = null,
                IsSeedAuthor = true,
                IsActive = true,
            },
            new Author
            {
                Id = 5,
                Name = "ThemeWiz",
                Bio = "Premium theme development studio creating high-quality templates for businesses worldwide. Our focus is on performance, accessibility, and user experience.",
                AvatarUrl = "~/src/img/person5.jpg",
                Email = "hello@themewiz.com",
                Website = "https://themewiz.com",
                SocialLinks = "{\"twitter\":\"@themewiz\",\"linkedin\":\"themewiz\",\"dribbble\":\"themewiz\"}",
                TotalProducts = 7,
                TotalDownloads = 13500,
                Specialization = "Premium Themes & SaaS",
                UserId = null,
                IsSeedAuthor = true,
                IsActive = true,
            },
            new Author
            {
                Id = 6,
                Name = "PixelGenius",
                Bio = "Creative design studio pushing the boundaries of digital design. We create unique, innovative templates that inspire and engage users across all platforms.",
                AvatarUrl = "~/src/img/person6.jpg",
                Email = "contact@pixelgenius.com",
                Website = "https://pixelgenius.com",
                SocialLinks = "{\"twitter\":\"@pixelgenius\",\"behance\":\"pixelgenius\",\"instagram\":\"pixelgenius\"}",
                TotalProducts = 9,
                TotalDownloads = 16800,
                Specialization = "Creative Design & Animation",
                UserId = null,
                IsSeedAuthor = true,
                IsActive = true,
            },
            new Author
            {
                Id = 7,
                Name = "WebMajestic",
                Bio = "Professional web development agency specializing in custom solutions and premium templates. We help businesses establish a strong online presence.",
                AvatarUrl = "~/src/img/person1.jpg",
                Email = "hello@webmajestic.com",
                Website = "https://webmajestic.com",
                SocialLinks = "{\"twitter\":\"@webmajestic\",\"linkedin\":\"webmajestic\",\"github\":\"webmajestic\"}",
                TotalProducts = 6,
                TotalDownloads = 11200,
                Specialization = "Web Development & Agency",
                UserId = null,
                IsSeedAuthor = true,
                IsActive = true,
            },
            new Author
            {
                Id = 8,
                Name = "TemplatePro",
                Bio = "Leading template marketplace with a focus on quality and innovation. Our team creates templates that help businesses grow and succeed online.",
                AvatarUrl = "~/src/img/person2.jpg",
                Email = "contact@templatepro.com",
                Website = "https://templatepro.com",
                SocialLinks = "{\"twitter\":\"@templatepro\",\"linkedin\":\"templatepro\",\"dribbble\":\"templatepro\"}",
                TotalProducts = 12,
                TotalDownloads = 22400,
                Specialization = "Template Development",
                UserId = null,
                IsSeedAuthor = true,
                IsActive = true,
            },
            new Author
            {
                Id = 9,
                Name = "WebStudio",
                Bio = "Modern web studio creating cutting-edge digital experiences. We combine creativity with technology to deliver exceptional results for our clients.",
                AvatarUrl = "~/src/img/person3.jpg",
                Email = "hello@webstudio.com",
                Website = "https://webstudio.com",
                SocialLinks = "{\"twitter\":\"@webstudio\",\"linkedin\":\"webstudio\",\"behance\":\"webstudio\"}",
                TotalProducts = 5,
                TotalDownloads = 8900,
                Specialization = "Modern Web Development",
                UserId = null,
                IsSeedAuthor = true,
                IsActive = true,
            }
        );
    }
}

