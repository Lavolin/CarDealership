using System.ComponentModel.DataAnnotations;

using static CarDealership.Infrastucture.Constants.DataConstants.MotorCategory;


namespace CarDealership.Infrastructure.Data
{
    public class MotorCategory
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(NameMaxLength)]
        public string Name { get; set; } = null!;

        public List<Motor> Motors { get; set; } = new List<Motor>();
    }
}
