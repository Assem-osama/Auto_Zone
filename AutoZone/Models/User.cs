using System.ComponentModel.DataAnnotations;
using static AutoZone.Models.Enum;
using System.Collections.Generic;


namespace AutoZone.Models
{
    public class User
    {
        [Key]
        public int Id {  get; set; }

        [Required]
        [MaxLength(100)]
        public string Name { get; set; }

        [Required]
        [MaxLength(100)]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        public string PasswordHash { get; set; }

        [MaxLength(20)]
        public string Phone { get; set; }

        public UserRole Role { get; set; }=UserRole.User;

        public ICollection<Car> Cars { get; set; } = new List<Car>();

        public ICollection<Rental> Rentals { get; set; } = new List<Rental>();


    }
}
