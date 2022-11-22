using System.ComponentModel.DataAnnotations;

using static CarDealership.Infrastucture.Constants.DataConstants.TruckCategory;

namespace CarDealership.Infrastructure.Data
{
    public class TruckCategory
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(NameMaxLength)]
        public string Name { get; set; } = null!;

        public List<Truck> Trucks { get; set; } = new List<Truck>();
    }
}
