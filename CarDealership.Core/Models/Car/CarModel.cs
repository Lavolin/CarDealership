using System.ComponentModel.DataAnnotations;

namespace CarDealership.Core.Models.Car
{
    public class CarModel
    {
        public int Id { get; set; }

        [Required]
        [StringLength(50, MinimumLength = 2)]
        public string Model { get; set; } = null!;


        [Required]
        [StringLength(500, MinimumLength = 10)]
        public string Description { get; set; } = null!;

        [Required]        
        [Display(Name = "Image URL")]
        public string ImageUrl { get; set; } = null!;

        [Required]
        [Display(Name = "Price")]
        [Range(0.00, 10000000.00, ErrorMessage = "The Price must be positive number")]
        public decimal Price { get; set; }

        [Display(Name = "Category")]
        public int CarCategoryId { get; set; }

        public IEnumerable<CarCategoryModel> CarCategories { get; set; } = new List<CarCategoryModel>();

    }
}
