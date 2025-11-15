namespace AutoZone.DTOs.Car
{
    public class UpdateCarDTO
    {
        public int Id { get; set; } // لازم نعرف انهي عربية هنعدلها
        public string? Brand { get; set; }
        public string? Model { get; set; }
        public int? Year { get; set; }
        public decimal? Price { get; set; }
        public string Description { get; set; }
        public bool? IsAvailable { get; set; }
    }
}
