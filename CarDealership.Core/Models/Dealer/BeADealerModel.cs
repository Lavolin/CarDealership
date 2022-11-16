using System.ComponentModel.DataAnnotations;

namespace CarDealership.Core.Models.Dealer
{
    public class BeADealerModel
    {
        [Required]
        [StringLength(15, MinimumLength = 8)]
        [Phone]
        [Display(Name = "Phone Number")]
        public string Phone { get; set; } = null!;
    }
}
