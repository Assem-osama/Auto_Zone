namespace AutoZone.DTOs.Car
{
    public class CreateCarDTO
    {
        public string Brand { get; set; }
        public string Model { get; set; }
        public int Year { get; set; }
        public decimal Price { get; set; }
        public string? Description { get; set; }

        public string ImageUrl { get; set; } = "default-car.jpg";
        public int OwnerId { get; set; }
    }
}
