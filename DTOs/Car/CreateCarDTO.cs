using System.ComponentModel.DataAnnotations;

namespace AutoZone.DTOs.Car
{
    public class CreateCarDTO
    {
        [Required(ErrorMessage = "Brand is required")]
        [StringLength(60, ErrorMessage = "Brand cannot exceed 60 characters")]
        public string Brand { get; set; }

        [Required(ErrorMessage = "Model is required")]
        [StringLength(60, ErrorMessage = "Model cannot exceed 60 characters")]
        public string Model { get; set; }

        [Range(1900, 2100, ErrorMessage = "Year must be between 1900 and 2100")]
        public int Year { get; set; }

        [Range(0, double.MaxValue, ErrorMessage = "Price must be a positive value")]
        public decimal Price { get; set; }

        [StringLength(500, ErrorMessage = "Description cannot exceed 500 characters")]
        public string? Description { get; set; }

        public string ImageUrl { get; set; } = "default-car.jpg";
        
        [Required]
        public int OwnerId { get; set; }
    }
}
