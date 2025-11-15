using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AutoZone.Models
{
    public class Rental
    {
        [Key] 
        public int Id { get; set; }

        [Required]
        public DateTime StartDate { get; set; }

        [Required] 
        public DateTime EndDate { get; set; }

        
        [Column(TypeName = "decimal(18,2)")]
        public decimal TotalPrice { get; set; }

        [Required]
        public int CarId { get; set; }

        [ForeignKey("CarId")]
        public Car Car { get; set; }



        [Required]
        public int RenterId { get; set; }

        [ForeignKey("RenterId")]
        public User Renter { get; set; }

        [NotMapped]//يعني متضيفهاش للجدول في الداتا بيز
        public int Days => (int)Math.Ceiling((EndDate.Date - StartDate.Date).TotalDays) == 0 ? 1 :
                      (int)Math.Ceiling((EndDate.Date - StartDate.Date).TotalDays);

    }
}
