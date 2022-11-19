using System.ComponentModel.DataAnnotations;

namespace CarDealership.Core.Models.Car
{
    public class CarServiceModel
    {
        public int Id { get; set; }

        public string Model { get; set; } = null!;        

        [Display(Name = "Image URL")]
        public string ImageUrl { get; set; } = null!;

        [Display(Name = "Price")]
        public decimal Price { get; set; }

        [Display(Name = "Is Bought")]
        public bool IsBought { get; set; }
    }
}
