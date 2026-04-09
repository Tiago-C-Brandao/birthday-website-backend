using BirthdayWebsiteAPI.Models.Enums;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace BirthdayWebsiteAPI.Models
{
    [Index(nameof(WhatsApp), IsUnique = true)]
    public class Guest
    {
        public int Id { get; set; }
        [Required]
        [MaxLength(100, ErrorMessage = "The name needs 50 characters")]
        public string FullName { get; set; }
        [Required]
        [RegularExpression(
            @"^(?:\+55\s?)?(?:\(?\d{2}\)?\s?)(?:9\d{4}|\d{4})-?\d{4}$",
            ErrorMessage = "WhatsApp invalid (use brazilian format with DDD)")]
        public string WhatsApp { get; set; }
        public string? UserId { get; set; }
        public string? AccompanyingBy { get; set; }
        [EnumDataType(typeof(GuestStatus))]
        public GuestStatus Status { get; set; } = GuestStatus.Confirmed;
        public DateTime CreatedAt { get; set; }
    }
}
