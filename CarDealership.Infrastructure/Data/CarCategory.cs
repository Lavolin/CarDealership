using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

using static CarDealership.Infrastucture.Constants.DataConstants.CarCategory;

namespace CarDealership.Infrastructure.Data
{
    public class CarCategory
    {
        [Key]
        public int Id { get; set; }

        [Comment("Category name")]
        [Required]
        [StringLength(NameMaxLength)]
        public string Name { get; set; } = null!;

        public List<Car> Cars { get; set; } = new List<Car>();
    }
}
