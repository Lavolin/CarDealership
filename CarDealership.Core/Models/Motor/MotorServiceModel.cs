using System.ComponentModel.DataAnnotations;

namespace CarDealership.Core.Models.Motor
{
    public class MotorServiceModel
    {
        public int Id { get; set; }

        public string Model { get; set; } = null!;

        [Display(Name = "Image URL")]
        public string ImageUrl { get; set; } = null!;

        [Display(Name = "Price")]
        public decimal Price { get; set; }

        [Display(Name = "Sold")]
        public bool IsBought { get; set; }
    }
}
