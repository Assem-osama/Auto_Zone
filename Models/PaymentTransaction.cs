using System;
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace AutoZone.Models
{
    public class PaymentTransaction
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "UserId is required")]
        public int UserId { get; set; }

        [Required(ErrorMessage = "CarId is required")]
        public int CarId { get; set; }

        [Required(ErrorMessage = "Amount is required")]
        [Range(1, long.MaxValue, ErrorMessage = "Amount must be greater than 0")]
        
        public long Amount { get; set; }

        [Required(ErrorMessage = "Currency is required")]
        [StringLength(10)]
        public string Currency { get; set; } = "usd";

        [Required]
        [StringLength(50)]
        public string StripeSessionId { get; set; } = string.Empty;

        [Required]
        [StringLength(20)]
        public string Status { get; set; } = "pending";// Pending, Paid, Failed

        [Required]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
