namespace AutoZone.DTOs.Rental
{
    public class CreateRentalDTO
    {
        public int CarId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        //الـ userId مش بيتبعت من الـ DTO لأننا بناخده من التوكن (Claim)،
        //بيتحسب السعر  جوه السيرفيس بعد ما يجيب سعر العربية
    }
}
