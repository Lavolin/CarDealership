using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

using static CarDealership.Infrastucture.Constants.DataConstants.Dealer;


namespace CarDealership.Infrastructure.Data
{
    public class Dealer
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(PhoneNumberLength)]
        public string PhoneNumber { get; set; } = null!;

        [Required]
        [ForeignKey(nameof(User))]
        public string UserId { get; set; } = null!;

        public ApplicationUser User { get; set; } = null!;
    }
}
