using System.ComponentModel.DataAnnotations;

namespace CarDealership.Infrastructure.Data
{
    public class TruckCategory
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(50)]
        public string Name { get; set; } = null!;

        public List<Truck> Trucks { get; set; } = new List<Truck>();
    }
}
