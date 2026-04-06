using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace BirthdayWebsiteAPI.Models
{
    [Index(nameof(WhatsApp), IsUnique = true)]
    public class User : IdentityUser
    {
        [Required]
        [MaxLength(100, ErrorMessage = "The name needs 100 characters")]
        public string FullName { get; set; }
        [Required]
        [RegularExpression(
            @"^\+55\s?(\(?\d{2}\)?\s?)?9\d{4}-\d{4}$",
            ErrorMessage = "WhatsApp number must be in the format +55 (XX) 9XXXX-XXXX or +55 XX 9XXXX-XXXX")]
        public string WhatsApp { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
