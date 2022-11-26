using System.ComponentModel.DataAnnotations;

using static CarDealership.Core.Constants.ModelConstant.MotorModel;

namespace CarDealership.Core.Models.Motor
{
    public class MotorModel
    {
        public int Id { get; set; }

        [Required]
        [StringLength(ModelMaxLength, MinimumLength = ModelMinLength)]
        public string Model { get; set; } = null!;


        [Required]
        [StringLength(DescriptionMaxLength, MinimumLength = DescriptionMinLength)]
        public string Description { get; set; } = null!;

        [Required]
        [Display(Name = "Image URL")]
        public string ImageUrl { get; set; } = null!;

        [Required]
        [Display(Name = "Price")]
        [Range(0.00, 10000000.00, ErrorMessage = "The Price must be positive number")]
        public decimal Price { get; set; }

        [Display(Name = "Category")]
        public int MotorCategoryId { get; set; }

        public IEnumerable<MotorCategoryModel> MotorCategories { get; set; } = new List<MotorCategoryModel>();
    }
}
