using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

using static CarDealership.Infrastucture.Constants.DataConstants.Car;

namespace CarDealership.Infrastructure.Data
{
    public class Car
    {
        [Key]
        [Comment("Car Identifier")]
        public int Id { get; set; }

        [Comment("Car Model")]
        [Required]
        [StringLength(ModelMaxLength)]
        public string Model { get; set; } = null!;

        [Comment("Car Description")]
        [Required]
        [StringLength(DescriptionMaxLength)]
        public string Description { get; set; } = null!;

        [Required]
        [StringLength(ImageUrlMaxLength)]
        public string ImageUrl { get; set; } = null!;

        [Required]
        [Column(TypeName = "money")]
        [Precision(18, 2)]
        public decimal Price { get; set; }

        [Required]
        [ForeignKey(nameof(CarCategory))]
        public int CarCategoryId { get; set; }

        public CarCategory CarCategory { get; set; } = null!;

        [Required]
        [ForeignKey(nameof(Dealer))]
        public int DealerId { get; set; }
        public Dealer Dealer { get; set; }

        [ForeignKey(nameof(Buyer))]
        public string? BuyerId { get; set; }

        public IdentityUser? Buyer { get; set; }

        public bool IsActive { get; set; } = true;
    }
}
