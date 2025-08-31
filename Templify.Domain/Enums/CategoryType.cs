using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Templify.Domain.Enums
{
    public enum CategoryType
    {
        [Display(Name = "Business")]
        Business = 1,
        
        [Display(Name = "3D Web")]
        ThreeDWeb = 2,
        
        [Display(Name = "Saas Platforms")]
        SaasPlatforms = 3,
        
        [Display(Name = "Agency")]
        Agency = 4,
        
        [Display(Name = "Portfolio Design")]
        PortfolioDesign = 5,
        
        [Display(Name = "Ecommerce")]
        Ecommerce = 6,
        
        [Display(Name = "Education")]
        Education = 7,
        
        [Display(Name = "Health")]
        Health = 8,
        
        [Display(Name = "Marketing")]
        Marketing = 9,
        
        [Display(Name = "Restaurant & Food")]
        RestaurantAndFood = 10,
        
        [Display(Name = "Gaming & Entertainment")]
        GamingAndEntertainment = 11,
        
        [Display(Name = "Real Estate")]
        RealEstate = 12
    }
}


