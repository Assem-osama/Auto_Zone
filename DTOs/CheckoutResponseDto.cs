namespace AutoZone.DTOs
{
    public class CheckoutResponseDto
    {
        public string SessionId { get; set; } = string.Empty;
        public string PubKey { get; set; } = string.Empty;
        public string Url { get; set; } = string.Empty;
    }
}
