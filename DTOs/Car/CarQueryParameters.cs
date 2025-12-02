namespace AutoZone.DTOs.Car
{
    public class CarQueryParameters
    {
        private const int MaxPageSize = 50;
        public int PageNumber { get; set; } = 1;
        
        private int _pageSize = 10;
        public int PageSize
        {
            get => _pageSize;
            set => _pageSize = (value > MaxPageSize) ? MaxPageSize : value;
        }

        public string? Brand { get; set; }
        public decimal? MinPrice { get; set; }
        public decimal? MaxPrice { get; set; }
        
        public string? SortBy { get; set; } // "price", "year", "brand"
        public bool SortDescending { get; set; } = false;
    }
}
