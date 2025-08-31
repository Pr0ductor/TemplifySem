using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Templify.Domain.Entities;
using Templify.Domain.Enums;

namespace Templify.Persistence.Configurations;

public class ProductConfiguration : IEntityTypeConfiguration<Product>
{
    public void Configure(EntityTypeBuilder<Product> builder)
    {
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Name).IsRequired().HasMaxLength(100);
        builder.Property(x => x.Description).HasMaxLength(2000);
        builder.Property(x => x.ImageUrl).HasMaxLength(300);
        builder.Property(x => x.Author).HasMaxLength(100);
        builder.Property(x => x.Category).HasMaxLength(100);
        builder.Property(x => x.CategoryType).IsRequired();
        builder.Property(x => x.Tags).HasMaxLength(200);
        
        // Foreign key relationship
        // Note: OnDelete(DeleteBehavior.Restrict) is used to prevent deletion of products from seed authors
        // For user-authors, products are manually deleted in DeleteUserAsync before author deletion
        builder.HasOne(x => x.AuthorEntity)
            .WithMany(a => a.Products)
            .HasForeignKey(x => x.AuthorId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasData(
            // Business Category (2 products)
            new Product
            {
                Id = 1,
                Name = "BusinessPro",
                Description = "Professional Business Platform designed for modern enterprises seeking a comprehensive digital presence. This premium template features a sophisticated design with advanced functionality for corporate management, client relations, and business operations. Built with the latest web technologies, it offers seamless integration capabilities and responsive design that adapts perfectly to all devices. The platform includes built-in analytics, project management tools, and customer relationship management features to streamline your business processes.",
                ImageUrl = "~/src/img/tem1.png",
                Price = 99,
                Author = "DesignVista",
                AuthorId = 1,
                Category = "Business",
                CategoryType = CategoryType.Business,
                Downloads = 4580,
                Tags = "business, professional, platform"
            },
            new Product
            {
                Id = 2,
                Name = "CorporateHub",
                Description = "Corporate Management System that revolutionizes how businesses handle their daily operations and team collaboration. This enterprise-grade template provides a centralized hub for managing projects, tracking performance metrics, and facilitating communication across departments. The intuitive interface makes it easy for teams to collaborate effectively while maintaining professional standards and brand consistency. With advanced security features and customizable workflows, CorporateHub ensures your business data remains protected while optimizing productivity.",
                ImageUrl = "~/src/img/tem2.png",
                Price = 89,
                Author = "DesignCraft",
                AuthorId = 2,
                Category = "Business",
                CategoryType = CategoryType.Business,
                Downloads = 3580,
                Tags = "corporate, management, system"
            },
            
            // 3D Web Category (2 products)
            new Product
            {
                Id = 3,
                Name = "3DWebStudio",
                Description = "3D Web Design Platform that brings your digital projects to life with stunning three-dimensional graphics and interactive elements. This cutting-edge template leverages modern WebGL technology to create immersive user experiences that captivate visitors and enhance engagement. Perfect for creative agencies, design studios, and innovative brands looking to showcase their work in a visually striking manner. The platform includes customizable 3D models, animation libraries, and performance optimization tools to ensure smooth rendering across all devices.",
                ImageUrl = "~/src/img/tem3.png",
                Price = 129,
                Author = "CodeMaven",
                AuthorId = 3,
                Category = "3D Web",
                CategoryType = CategoryType.ThreeDWeb,
                Downloads = 2980,
                Tags = "3d, web, design"
            },
            new Product
            {
                Id = 4,
                Name = "ThreeDimensional",
                Description = "3D Interactive Experience that transforms ordinary websites into extraordinary digital journeys through immersive three-dimensional environments. This revolutionary template combines advanced 3D rendering with intuitive user interfaces to create memorable brand experiences that leave lasting impressions. Built for performance and accessibility, it ensures smooth navigation and fast loading times while delivering stunning visual effects. Ideal for gaming companies, entertainment platforms, and forward-thinking brands who want to push the boundaries of web design.",
                ImageUrl = "~/src/img/tem4.png",
                Price = 149,
                Author = "WebMaster",
                AuthorId = 4,
                Category = "3D Web",
                CategoryType = CategoryType.ThreeDWeb,
                Downloads = 2504,
                Tags = "3d, interactive, experience"
            },
            
            // Saas Platforms Category (2 products)
            new Product
            {
                Id = 5,
                Name = "CloudSaaS",
                Description = "Cloud-based SaaS Platform that provides a scalable foundation for software-as-a-service applications with enterprise-grade reliability and performance. This comprehensive template includes user authentication, subscription management, and analytics dashboards to help you build and grow your SaaS business efficiently. The modular architecture allows for easy customization and integration with third-party services, while the responsive design ensures optimal user experience across all devices. Built with security best practices and cloud-native technologies for maximum scalability and reliability.",
                ImageUrl = "~/src/img/tem5.png",
                Price = 199,
                Author = "ThemeWiz",
                AuthorId = 5,
                Category = "Saas Platforms",
                CategoryType = CategoryType.SaasPlatforms,
                Downloads = 1805,
                Tags = "saas, cloud, platform"
            },
            new Product
            {
                Id = 6,
                Name = "SaaSPro",
                Description = "Professional SaaS Solution designed to accelerate your software business with a complete toolkit for subscription management and user engagement. This feature-rich template includes advanced billing systems, user onboarding flows, and comprehensive analytics to help you understand and optimize your customer journey. The clean, modern interface focuses on user experience while providing powerful backend capabilities for managing complex SaaS operations. With built-in A/B testing tools and conversion optimization features, SaaSPro helps you maximize your revenue potential.",
                ImageUrl = "~/src/img/tem6.png",
                Price = 179,
                Author = "PixelGenius",
                AuthorId = 6,
                Category = "Saas Platforms",
                CategoryType = CategoryType.SaasPlatforms,
                Downloads = 1506,
                Tags = "saas, professional, solution"
            },
            
            // Agency Category (2 products)
            new Product
            {
                Id = 7,
                Name = "AgencyHub",
                Description = "Digital Agency Platform that showcases your creative work and attracts new clients with a professional, portfolio-focused design. This comprehensive template includes project galleries, client testimonials, and service showcases to highlight your agency's capabilities and past successes. The integrated contact forms and lead generation tools help convert visitors into clients, while the blog and content management features allow you to establish thought leadership in your industry. Built for creative agencies, marketing firms, and design studios who want to make a lasting impression.",
                ImageUrl = "~/src/img/tem7.png",
                Price = 159,
                Author = "WebMajestic",
                AuthorId = 7,
                Category = "Agency",
                CategoryType = CategoryType.Agency,
                Downloads = 1200,
                Tags = "agency, digital, platform"
            },
            new Product
            {
                Id = 8,
                Name = "CreativeAgency",
                Description = "Creative Agency Website that reflects the innovative spirit and artistic vision of modern creative businesses. This visually stunning template combines bold typography, dynamic layouts, and interactive elements to create a memorable brand experience that sets your agency apart from the competition. The portfolio section showcases your best work with elegant galleries and case studies, while the team profiles and about sections help build trust and credibility with potential clients. Perfect for advertising agencies, design studios, and creative consultancies looking to make a bold statement.",
                ImageUrl = "~/src/img/tem8.png",
                Price = 139,
                Author = "TemplatePro",
                AuthorId = 8,
                Category = "Agency",
                CategoryType = CategoryType.Agency,
                Downloads = 1100,
                Tags = "creative, agency, website"
            },
            
            // Portfolio Design Category (2 products)
            new Product
            {
                Id = 9,
                Name = "PortfolioPro",
                Description = "Professional Portfolio template that helps creative professionals showcase their work in the most compelling and organized way possible. This elegant design focuses on visual storytelling with large image galleries, smooth transitions, and intuitive navigation that guides visitors through your creative journey. The customizable sections allow you to highlight different types of work, from graphic design and photography to web development and branding projects. Built with SEO optimization and social media integration to help you reach a wider audience and attract new opportunities.",
                ImageUrl = "~/src/img/tem9.png",
                Price = 79,
                Author = "PixelGenius",
                AuthorId = 6,
                Category = "Portfolio Design",
                CategoryType = CategoryType.PortfolioDesign,
                Downloads = 900,
                Tags = "portfolio, professional, design"
            },
            new Product
            {
                Id = 10,
                Name = "DesignPortfolio",
                Description = "Creative Design Portfolio that celebrates artistic expression and showcases your unique creative vision with a modern, minimalist approach. This template emphasizes visual impact with full-screen galleries, custom animations, and typography that complements your work without overwhelming it. The flexible layout system allows you to create custom presentations for different types of projects, from branding campaigns to digital illustrations and user interface designs. Perfect for designers, illustrators, and creative professionals who want their work to speak for itself.",
                ImageUrl = "~/src/img/tem10.png",
                Price = 69,
                Author = "DesignVista",
                AuthorId = 1,
                Category = "Portfolio Design",
                CategoryType = CategoryType.PortfolioDesign,
                Downloads = 800,
                Tags = "design, portfolio, creative"
            },
            
            // Ecommerce Category (2 products)
            new Product
            {
                Id = 11,
                Name = "EcomStore",
                Description = "E-commerce Store Platform that provides everything you need to launch and grow your online business with a professional, conversion-optimized design. This comprehensive template includes product catalogs, shopping cart functionality, secure checkout processes, and inventory management tools to streamline your e-commerce operations. The mobile-responsive design ensures customers can shop easily on any device, while the integrated payment gateways and shipping calculators provide a seamless purchasing experience. Built with performance and security in mind to handle high traffic and protect customer data.",
                ImageUrl = "~/src/img/tem11.png",
                Price = 189,
                Author = "TemplatePro",
                AuthorId = 8,
                Category = "Ecommerce",
                CategoryType = CategoryType.Ecommerce,
                Downloads = 700,
                Tags = "ecommerce, store, platform"
            },
            new Product
            {
                Id = 12,
                Name = "ShopPro",
                Description = "Professional Shopping Site that combines elegant design with powerful e-commerce functionality to create an exceptional online shopping experience. This feature-rich template includes advanced product filtering, wishlist functionality, customer reviews, and personalized recommendations to increase sales and customer satisfaction. The integrated analytics dashboard helps you track performance metrics and optimize your store for better conversions. Perfect for fashion retailers, electronics stores, and specialty shops looking to create a premium shopping experience that builds customer loyalty.",
                ImageUrl = "~/src/img/tem12.png",
                Price = 169,
                Author = "WebStudio",
                AuthorId = 9,
                Category = "Ecommerce",
                CategoryType = CategoryType.Ecommerce,
                Downloads = 600,
                Tags = "shop, professional, ecommerce"
            },
            
            // Education Category (2 products)
            new Product
            {
                Id = 13,
                Name = "EduPlatform",
                Description = "Educational Platform designed to facilitate online learning and knowledge sharing with an intuitive, student-friendly interface. This comprehensive template includes course catalogs, video streaming capabilities, interactive quizzes, and progress tracking tools to create engaging learning experiences. The integrated discussion forums and messaging systems foster collaboration between students and instructors, while the analytics dashboard helps educators monitor student progress and course effectiveness. Built for universities, training centers, and online education providers who want to deliver high-quality learning experiences.",
                ImageUrl = "~/src/img/tem1.png",
                Price = 119,
                Author = "DesignVista",
                AuthorId = 1,
                Category = "Education",
                CategoryType = CategoryType.Education,
                Downloads = 550,
                Tags = "education, platform, learning"
            },
            new Product
            {
                Id = 14,
                Name = "LearnHub",
                Description = "Learning Management System that streamlines educational administration and enhances the learning experience for both students and instructors. This powerful template provides comprehensive tools for course creation, student enrollment, assignment management, and grade tracking in a user-friendly interface. The platform supports various content types including videos, documents, and interactive materials, while the built-in communication tools facilitate effective teacher-student interaction. Ideal for schools, corporate training programs, and educational institutions looking to modernize their learning delivery methods.",
                ImageUrl = "~/src/img/tem2.png",
                Price = 109,
                Author = "DesignCraft",
                AuthorId = 2,
                Category = "Education",
                CategoryType = CategoryType.Education,
                Downloads = 480,
                Tags = "learning, management, system"
            },
            
            // Health Category (2 products)
            new Product
            {
                Id = 15,
                Name = "HealthCare",
                Description = "Healthcare Platform that provides a secure, user-friendly interface for medical professionals and patients to manage healthcare services efficiently. This HIPAA-compliant template includes patient portals, appointment scheduling systems, medical record management, and telemedicine capabilities to support modern healthcare delivery. The platform prioritizes data security and privacy while offering intuitive navigation for users of all technical levels. Built for hospitals, clinics, and healthcare providers who want to digitize their services and improve patient care through technology.",
                ImageUrl = "~/src/img/tem3.png",
                Price = 159,
                Author = "CodeMaven",
                AuthorId = 3,
                Category = "Health",
                CategoryType = CategoryType.Health,
                Downloads = 420,
                Tags = "health, care, platform"
            },
            new Product
            {
                Id = 16,
                Name = "MedicalHub",
                Description = "Medical Services Website that establishes trust and credibility while providing essential information and services to patients and healthcare seekers. This professional template features doctor profiles, service descriptions, appointment booking systems, and educational content to help patients make informed healthcare decisions. The clean, reassuring design creates a calming environment that reduces patient anxiety while maintaining professional medical standards. Perfect for medical practices, dental offices, and healthcare facilities looking to build strong patient relationships and improve service accessibility.",
                ImageUrl = "~/src/img/tem4.png",
                Price = 149,
                Author = "WebMaster",
                AuthorId = 4,
                Category = "Health",
                CategoryType = CategoryType.Health,
                Downloads = 380,
                Tags = "medical, services, health"
            },
            
            // Marketing Category (2 products)
            new Product
            {
                Id = 17,
                Name = "MarketingPro",
                Description = "Marketing Agency Platform that demonstrates your marketing expertise and attracts clients with compelling case studies and results-driven content. This conversion-focused template includes lead generation forms, client testimonials, service showcases, and ROI tracking tools to prove your marketing effectiveness. The integrated analytics and reporting features help you demonstrate value to clients while the blog and content marketing tools establish your agency as an industry thought leader. Built for marketing agencies, digital consultancies, and growth hackers who want to showcase their results and attract high-value clients.",
                ImageUrl = "~/src/img/tem5.png",
                Price = 139,
                Author = "ThemeWiz",
                AuthorId = 5,
                Category = "Marketing",
                CategoryType = CategoryType.Marketing,
                Downloads = 350,
                Tags = "marketing, agency, platform"
            },
            new Product
            {
                Id = 18,
                Name = "AdCampaign",
                Description = "Advertising Campaign Site that creates buzz and generates excitement around your products, services, or events with compelling storytelling and interactive elements. This dynamic template features countdown timers, social media integration, viral sharing tools, and conversion optimization features to maximize campaign effectiveness. The mobile-first design ensures your campaigns reach audiences wherever they are, while the integrated analytics help you track performance and optimize results in real-time. Perfect for product launches, event promotions, and brand awareness campaigns that need to make a big impact.",
                ImageUrl = "~/src/img/tem6.png",
                Price = 129,
                Author = "PixelGenius",
                AuthorId = 6,
                Category = "Marketing",
                CategoryType = CategoryType.Marketing,
                Downloads = 320,
                Tags = "advertising, campaign, marketing"
            },
            
            // Restaurant & Food Category (2 products)
            new Product
            {
                Id = 19,
                Name = "RestaurantHub",
                Description = "Restaurant Management Platform that streamlines operations and enhances customer experience with online ordering, table reservations, and inventory management tools. This comprehensive template includes menu management, order tracking, customer feedback systems, and loyalty programs to help restaurants increase sales and build customer relationships. The mobile-responsive design ensures customers can easily browse menus, place orders, and make reservations from any device. Built for restaurants, cafes, and food service businesses looking to modernize their operations and improve customer satisfaction.",
                ImageUrl = "~/src/img/tem7.png",
                Price = 119,
                Author = "WebMajestic",
                AuthorId = 7,
                Category = "Restaurant & Food",
                CategoryType = CategoryType.RestaurantAndFood,
                Downloads = 290,
                Tags = "restaurant, food, management"
            },
            new Product
            {
                Id = 20,
                Name = "FoodDelivery",
                Description = "Food Delivery Service platform that connects restaurants with customers through an intuitive ordering system and real-time delivery tracking. This feature-rich template includes restaurant listings, menu browsing, secure payment processing, and delivery status updates to create a seamless food ordering experience. The integrated driver management system and route optimization tools help ensure timely deliveries while the customer review system builds trust and helps restaurants improve their service. Perfect for food delivery startups, restaurant chains, and meal kit services looking to expand their digital presence.",
                ImageUrl = "~/src/img/tem8.png",
                Price = 109,
                Author = "TemplatePro",
                AuthorId = 8,
                Category = "Restaurant & Food",
                CategoryType = CategoryType.RestaurantAndFood,
                Downloads = 260,
                Tags = "food, delivery, service"
            },
            
            // Gaming & Entertainment Category (2 products)
            new Product
            {
                Id = 21,
                Name = "GameStudio",
                Description = "Gaming Studio Platform that showcases your games and connects with players through an immersive, gaming-focused design that reflects the excitement of interactive entertainment. This dynamic template includes game galleries, development blogs, community forums, and player feedback systems to build a strong gaming community around your studio. The integrated streaming capabilities and social media features help promote your games and engage with players across multiple platforms. Built for indie game developers, gaming studios, and esports organizations looking to build passionate player communities.",
                ImageUrl = "~/src/img/tem9.png",
                Price = 169,
                Author = "PixelGenius",
                AuthorId = 6,
                Category = "Gaming & Entertainment",
                CategoryType = CategoryType.GamingAndEntertainment,
                Downloads = 240,
                Tags = "gaming, studio, platform"
            },
            new Product
            {
                Id = 22,
                Name = "EntertainmentHub",
                Description = "Entertainment Website that captivates audiences with stunning visuals, engaging content, and interactive features that bring entertainment to life online. This multimedia-rich template includes video streaming capabilities, event calendars, artist profiles, and fan interaction tools to create immersive entertainment experiences. The responsive design ensures optimal viewing across all devices while the integrated social features help build fan communities and increase engagement. Perfect for entertainment companies, streaming platforms, and content creators looking to deliver compelling digital experiences.",
                ImageUrl = "~/src/img/tem10.png",
                Price = 159,
                Author = "DesignVista",
                AuthorId = 1,
                Category = "Gaming & Entertainment",
                CategoryType = CategoryType.GamingAndEntertainment,
                Downloads = 220,
                Tags = "entertainment, gaming, website"
            },
            
            // Real Estate Category (2 products)
            new Product
            {
                Id = 23,
                Name = "RealEstatePro",
                Description = "Real Estate Platform that helps agents and agencies showcase properties effectively and connect with potential buyers through comprehensive listing management and search tools. This professional template includes property galleries, virtual tour capabilities, mortgage calculators, and client relationship management features to streamline the real estate process. The advanced search and filtering options help buyers find their perfect property while the integrated lead generation tools help agents convert visitors into clients. Built for real estate agencies, property developers, and individual agents looking to modernize their business.",
                ImageUrl = "~/src/img/tem11.png",
                Price = 179,
                Author = "TemplatePro",
                AuthorId = 8,
                Category = "Real Estate",
                CategoryType = CategoryType.RealEstate,
                Downloads = 200,
                Tags = "real estate, property, platform"
            },
            new Product
            {
                Id = 24,
                Name = "PropertyHub",
                Description = "Property Management System that simplifies property administration and tenant communication through an integrated platform designed for property managers and landlords. This comprehensive template includes tenant portals, maintenance request systems, rent collection tools, and property analytics to help manage real estate investments efficiently. The secure messaging system and document management features facilitate clear communication between property managers and tenants while the financial tracking tools help monitor property performance. Perfect for property management companies, real estate investors, and landlords looking to streamline their operations.",
                ImageUrl = "~/src/img/tem12.png",
                Price = 169,
                Author = "WebStudio",
                AuthorId = 9,
                Category = "Real Estate",
                CategoryType = CategoryType.RealEstate,
                Downloads = 180,
                Tags = "property, management, real estate"
            }
        );
    }
}
              
