namespace AutoZone.DTOs
{
    public class CreateCheckoutSessionDto
    {
        public int UserId { get; set; }

        public int CarId { get; set; }

        public string SuccessUrl { get; set; } = string.Empty;

        public string CancelUrl { get; set; } = string.Empty;
        public long Amount { get; set; }

        public string Currency { get; set; } = "usd";
    }

}
