namespace AutoZone.DTOs.Car
{
    public class CarDTO
    {
        public int Id { get; set; }
        public string Brand { get; set; }  // الشركة المصنعة
        public string Model { get; set; } // الموديل
        public int Year { get; set; }
        public decimal Price { get; set; }
        public string? Description { get; set; }
        public bool IsAvailable { get; set; }
        public int OwnerId { get; set; } // صاحب العربية (UserId)
    }
}
