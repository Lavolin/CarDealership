using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

using static CarDealership.Infrastucture.Constants.DataConstants.Truck;


namespace CarDealership.Infrastructure.Data
{
    public class Truck
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(ModelMaxLength)]
        public string Model { get; set; } = null!;

        
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
        [ForeignKey(nameof(TruckCategory))]
        public int TruckCategoryId { get; set; }

        public TruckCategory TruckCategory { get; set; } = null!;

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
