using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Numerics;
using static AutoZone.Models.Enum;

namespace AutoZone.Models
{
    public class Car
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(60)]
        public string Brand { get; set; }
        
        [Required]
        [MaxLength(60)] 
        public string Model { get; set; }

        public int Year { get; set; }


        [Column(TypeName = "decimal(18,2)")]
        public decimal? Price { get; set; }


        [Column(TypeName = "decimal(18,2)")]
        public decimal? PricePerDay { get; set; }

        public CarStatus status { get; set; } = CarStatus.ForSale;

        public string ImageUrl { get; set; }
        
        [MaxLength(500)]
        public string Description { get; set; }

        [Required]
        public int UserId { get; set; }

        [ForeignKey("UserId")]
        public User Owner { get; set; }

        public ICollection<Rental> Rentals { get; set; } = new List<Rental>();

    }

}
