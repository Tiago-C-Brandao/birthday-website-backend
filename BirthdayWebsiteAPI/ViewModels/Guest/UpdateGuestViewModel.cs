using System.ComponentModel.DataAnnotations;

namespace BirthdayWebsiteAPI.ViewModels.Guest
{
    public class UpdateGuestViewModel
    {
        [MaxLength(100, ErrorMessage = "The name needs 100 characters")]
        public string? FullName { get; set; }
        [RegularExpression(@"^\(?\d{2}\)?\s?9\d{4}-\d{4}$", ErrorMessage = "WhatsApp number must be in the format (XX) 9XXXX-XXXX or XX 9XXXX-XXXX")]
        public string? WhatsApp { get; set; }
        public string? UserId { get; set; }
        public string? AccompanyingBy { get; set; }
    }
}
