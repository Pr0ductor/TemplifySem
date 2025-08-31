using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Templify.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class UpdateProductDescriptions : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "Products",
                type: "character varying(2000)",
                maxLength: 2000,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(1000)",
                oldMaxLength: 1000);

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 1,
                column: "Description",
                value: "Professional Business Platform designed for modern enterprises seeking a comprehensive digital presence. This premium template features a sophisticated design with advanced functionality for corporate management, client relations, and business operations. Built with the latest web technologies, it offers seamless integration capabilities and responsive design that adapts perfectly to all devices. The platform includes built-in analytics, project management tools, and customer relationship management features to streamline your business processes.");

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 2,
                column: "Description",
                value: "Corporate Management System that revolutionizes how businesses handle their daily operations and team collaboration. This enterprise-grade template provides a centralized hub for managing projects, tracking performance metrics, and facilitating communication across departments. The intuitive interface makes it easy for teams to collaborate effectively while maintaining professional standards and brand consistency. With advanced security features and customizable workflows, CorporateHub ensures your business data remains protected while optimizing productivity.");

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 3,
                column: "Description",
                value: "3D Web Design Platform that brings your digital projects to life with stunning three-dimensional graphics and interactive elements. This cutting-edge template leverages modern WebGL technology to create immersive user experiences that captivate visitors and enhance engagement. Perfect for creative agencies, design studios, and innovative brands looking to showcase their work in a visually striking manner. The platform includes customizable 3D models, animation libraries, and performance optimization tools to ensure smooth rendering across all devices.");

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 4,
                column: "Description",
                value: "3D Interactive Experience that transforms ordinary websites into extraordinary digital journeys through immersive three-dimensional environments. This revolutionary template combines advanced 3D rendering with intuitive user interfaces to create memorable brand experiences that leave lasting impressions. Built for performance and accessibility, it ensures smooth navigation and fast loading times while delivering stunning visual effects. Ideal for gaming companies, entertainment platforms, and forward-thinking brands who want to push the boundaries of web design.");

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 5,
                column: "Description",
                value: "Cloud-based SaaS Platform that provides a scalable foundation for software-as-a-service applications with enterprise-grade reliability and performance. This comprehensive template includes user authentication, subscription management, and analytics dashboards to help you build and grow your SaaS business efficiently. The modular architecture allows for easy customization and integration with third-party services, while the responsive design ensures optimal user experience across all devices. Built with security best practices and cloud-native technologies for maximum scalability and reliability.");

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 6,
                column: "Description",
                value: "Professional SaaS Solution designed to accelerate your software business with a complete toolkit for subscription management and user engagement. This feature-rich template includes advanced billing systems, user onboarding flows, and comprehensive analytics to help you understand and optimize your customer journey. The clean, modern interface focuses on user experience while providing powerful backend capabilities for managing complex SaaS operations. With built-in A/B testing tools and conversion optimization features, SaaSPro helps you maximize your revenue potential.");

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 7,
                column: "Description",
                value: "Digital Agency Platform that showcases your creative work and attracts new clients with a professional, portfolio-focused design. This comprehensive template includes project galleries, client testimonials, and service showcases to highlight your agency's capabilities and past successes. The integrated contact forms and lead generation tools help convert visitors into clients, while the blog and content management features allow you to establish thought leadership in your industry. Built for creative agencies, marketing firms, and design studios who want to make a lasting impression.");

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 8,
                column: "Description",
                value: "Creative Agency Website that reflects the innovative spirit and artistic vision of modern creative businesses. This visually stunning template combines bold typography, dynamic layouts, and interactive elements to create a memorable brand experience that sets your agency apart from the competition. The portfolio section showcases your best work with elegant galleries and case studies, while the team profiles and about sections help build trust and credibility with potential clients. Perfect for advertising agencies, design studios, and creative consultancies looking to make a bold statement.");

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 9,
                column: "Description",
                value: "Professional Portfolio template that helps creative professionals showcase their work in the most compelling and organized way possible. This elegant design focuses on visual storytelling with large image galleries, smooth transitions, and intuitive navigation that guides visitors through your creative journey. The customizable sections allow you to highlight different types of work, from graphic design and photography to web development and branding projects. Built with SEO optimization and social media integration to help you reach a wider audience and attract new opportunities.");

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 10,
                column: "Description",
                value: "Creative Design Portfolio that celebrates artistic expression and showcases your unique creative vision with a modern, minimalist approach. This template emphasizes visual impact with full-screen galleries, custom animations, and typography that complements your work without overwhelming it. The flexible layout system allows you to create custom presentations for different types of projects, from branding campaigns to digital illustrations and user interface designs. Perfect for designers, illustrators, and creative professionals who want their work to speak for itself.");

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 11,
                column: "Description",
                value: "E-commerce Store Platform that provides everything you need to launch and grow your online business with a professional, conversion-optimized design. This comprehensive template includes product catalogs, shopping cart functionality, secure checkout processes, and inventory management tools to streamline your e-commerce operations. The mobile-responsive design ensures customers can shop easily on any device, while the integrated payment gateways and shipping calculators provide a seamless purchasing experience. Built with performance and security in mind to handle high traffic and protect customer data.");

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 12,
                column: "Description",
                value: "Professional Shopping Site that combines elegant design with powerful e-commerce functionality to create an exceptional online shopping experience. This feature-rich template includes advanced product filtering, wishlist functionality, customer reviews, and personalized recommendations to increase sales and customer satisfaction. The integrated analytics dashboard helps you track performance metrics and optimize your store for better conversions. Perfect for fashion retailers, electronics stores, and specialty shops looking to create a premium shopping experience that builds customer loyalty.");

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 13,
                column: "Description",
                value: "Educational Platform designed to facilitate online learning and knowledge sharing with an intuitive, student-friendly interface. This comprehensive template includes course catalogs, video streaming capabilities, interactive quizzes, and progress tracking tools to create engaging learning experiences. The integrated discussion forums and messaging systems foster collaboration between students and instructors, while the analytics dashboard helps educators monitor student progress and course effectiveness. Built for universities, training centers, and online education providers who want to deliver high-quality learning experiences.");

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 14,
                column: "Description",
                value: "Learning Management System that streamlines educational administration and enhances the learning experience for both students and instructors. This powerful template provides comprehensive tools for course creation, student enrollment, assignment management, and grade tracking in a user-friendly interface. The platform supports various content types including videos, documents, and interactive materials, while the built-in communication tools facilitate effective teacher-student interaction. Ideal for schools, corporate training programs, and educational institutions looking to modernize their learning delivery methods.");

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 15,
                column: "Description",
                value: "Healthcare Platform that provides a secure, user-friendly interface for medical professionals and patients to manage healthcare services efficiently. This HIPAA-compliant template includes patient portals, appointment scheduling systems, medical record management, and telemedicine capabilities to support modern healthcare delivery. The platform prioritizes data security and privacy while offering intuitive navigation for users of all technical levels. Built for hospitals, clinics, and healthcare providers who want to digitize their services and improve patient care through technology.");

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 16,
                column: "Description",
                value: "Medical Services Website that establishes trust and credibility while providing essential information and services to patients and healthcare seekers. This professional template features doctor profiles, service descriptions, appointment booking systems, and educational content to help patients make informed healthcare decisions. The clean, reassuring design creates a calming environment that reduces patient anxiety while maintaining professional medical standards. Perfect for medical practices, dental offices, and healthcare facilities looking to build strong patient relationships and improve service accessibility.");

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 17,
                column: "Description",
                value: "Marketing Agency Platform that demonstrates your marketing expertise and attracts clients with compelling case studies and results-driven content. This conversion-focused template includes lead generation forms, client testimonials, service showcases, and ROI tracking tools to prove your marketing effectiveness. The integrated analytics and reporting features help you demonstrate value to clients while the blog and content marketing tools establish your agency as an industry thought leader. Built for marketing agencies, digital consultancies, and growth hackers who want to showcase their results and attract high-value clients.");

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 18,
                column: "Description",
                value: "Advertising Campaign Site that creates buzz and generates excitement around your products, services, or events with compelling storytelling and interactive elements. This dynamic template features countdown timers, social media integration, viral sharing tools, and conversion optimization features to maximize campaign effectiveness. The mobile-first design ensures your campaigns reach audiences wherever they are, while the integrated analytics help you track performance and optimize results in real-time. Perfect for product launches, event promotions, and brand awareness campaigns that need to make a big impact.");

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 19,
                column: "Description",
                value: "Restaurant Management Platform that streamlines operations and enhances customer experience with online ordering, table reservations, and inventory management tools. This comprehensive template includes menu management, order tracking, customer feedback systems, and loyalty programs to help restaurants increase sales and build customer relationships. The mobile-responsive design ensures customers can easily browse menus, place orders, and make reservations from any device. Built for restaurants, cafes, and food service businesses looking to modernize their operations and improve customer satisfaction.");

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 20,
                column: "Description",
                value: "Food Delivery Service platform that connects restaurants with customers through an intuitive ordering system and real-time delivery tracking. This feature-rich template includes restaurant listings, menu browsing, secure payment processing, and delivery status updates to create a seamless food ordering experience. The integrated driver management system and route optimization tools help ensure timely deliveries while the customer review system builds trust and helps restaurants improve their service. Perfect for food delivery startups, restaurant chains, and meal kit services looking to expand their digital presence.");

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 21,
                column: "Description",
                value: "Gaming Studio Platform that showcases your games and connects with players through an immersive, gaming-focused design that reflects the excitement of interactive entertainment. This dynamic template includes game galleries, development blogs, community forums, and player feedback systems to build a strong gaming community around your studio. The integrated streaming capabilities and social media features help promote your games and engage with players across multiple platforms. Built for indie game developers, gaming studios, and esports organizations looking to build passionate player communities.");

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 22,
                column: "Description",
                value: "Entertainment Website that captivates audiences with stunning visuals, engaging content, and interactive features that bring entertainment to life online. This multimedia-rich template includes video streaming capabilities, event calendars, artist profiles, and fan interaction tools to create immersive entertainment experiences. The responsive design ensures optimal viewing across all devices while the integrated social features help build fan communities and increase engagement. Perfect for entertainment companies, streaming platforms, and content creators looking to deliver compelling digital experiences.");

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 23,
                column: "Description",
                value: "Real Estate Platform that helps agents and agencies showcase properties effectively and connect with potential buyers through comprehensive listing management and search tools. This professional template includes property galleries, virtual tour capabilities, mortgage calculators, and client relationship management features to streamline the real estate process. The advanced search and filtering options help buyers find their perfect property while the integrated lead generation tools help agents convert visitors into clients. Built for real estate agencies, property developers, and individual agents looking to modernize their business.");

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 24,
                column: "Description",
                value: "Property Management System that simplifies property administration and tenant communication through an integrated platform designed for property managers and landlords. This comprehensive template includes tenant portals, maintenance request systems, rent collection tools, and property analytics to help manage real estate investments efficiently. The secure messaging system and document management features facilitate clear communication between property managers and tenants while the financial tracking tools help monitor property performance. Perfect for property management companies, real estate investors, and landlords looking to streamline their operations.");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "Products",
                type: "character varying(1000)",
                maxLength: 1000,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(2000)",
                oldMaxLength: 2000);

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 1,
                column: "Description",
                value: "Professional Business Platform");

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 2,
                column: "Description",
                value: "Corporate Management System");

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 3,
                column: "Description",
                value: "3D Web Design Platform");

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 4,
                column: "Description",
                value: "3D Interactive Experience");

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 5,
                column: "Description",
                value: "Cloud-based SaaS Platform");

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 6,
                column: "Description",
                value: "Professional SaaS Solution");

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 7,
                column: "Description",
                value: "Digital Agency Platform");

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 8,
                column: "Description",
                value: "Creative Agency Website");

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 9,
                column: "Description",
                value: "Professional Portfolio");

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 10,
                column: "Description",
                value: "Creative Design Portfolio");

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 11,
                column: "Description",
                value: "E-commerce Store Platform");

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 12,
                column: "Description",
                value: "Professional Shopping Site");

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 13,
                column: "Description",
                value: "Educational Platform");

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 14,
                column: "Description",
                value: "Learning Management System");

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 15,
                column: "Description",
                value: "Healthcare Platform");

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 16,
                column: "Description",
                value: "Medical Services Website");

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 17,
                column: "Description",
                value: "Marketing Agency Platform");

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 18,
                column: "Description",
                value: "Advertising Campaign Site");

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 19,
                column: "Description",
                value: "Restaurant Management Platform");

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 20,
                column: "Description",
                value: "Food Delivery Service");

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 21,
                column: "Description",
                value: "Gaming Studio Platform");

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 22,
                column: "Description",
                value: "Entertainment Website");

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 23,
                column: "Description",
                value: "Real Estate Platform");

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 24,
                column: "Description",
                value: "Property Management System");
        }
    }
}
