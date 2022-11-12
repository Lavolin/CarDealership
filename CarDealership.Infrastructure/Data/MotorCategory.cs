using System.ComponentModel.DataAnnotations;

namespace CarDealership.Infrastructure.Data
{
    public class MotorCategory
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(50)]
        public string Name { get; set; } = null!;

        public List<Motor> Motors { get; set; } = new List<Motor>();
    }
}
