using AutoZone.DTOs.Car;

namespace AutoZone.DTOs.Rental
{
    public class RentalDTO
    {
        public int Id { get; set; }
        public int CarId { get; set; }
        public CarDTO Car { get; set; }
        public UserResponseDTO Renter { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public decimal TotalPrice { get; set; }
    }
}
