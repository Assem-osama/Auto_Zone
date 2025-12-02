using System.ComponentModel.DataAnnotations;

namespace AutoZone.DTOs.Rental
{
    public class CreateRentalDTO
    {
        [Required(ErrorMessage = "CarId is required")]
        public int CarId { get; set; }

        [Required(ErrorMessage = "Start Date is required")]
        public DateTime StartDate { get; set; }

        [Required(ErrorMessage = "End Date is required")]
        public DateTime EndDate { get; set; }

        //الـ userId مش بيتبعت من الـ DTO لأننا بناخده من التوكن (Claim)،
        //بيتحسب السعر  جوه السيرفيس بعد ما يجيب سعر العربية
    }
}
