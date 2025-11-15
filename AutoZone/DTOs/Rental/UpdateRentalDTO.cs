namespace AutoZone.DTOs.Rental
{
    public class UpdateRentalDTO
    {
        
        public int CarId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

        //بيتحسب السعر الجديد جوه السيرفيس بعد ما يجيب سعر العربية


    }
}
