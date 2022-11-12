using System.ComponentModel.DataAnnotations;

namespace CarDealership.Infrastructure.Data
{
    public class CarCategory
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(50)]
        public string Name { get; set; } = null!;

        public List<Car> Cars { get; set; } = new List<Car>();
    }
}
