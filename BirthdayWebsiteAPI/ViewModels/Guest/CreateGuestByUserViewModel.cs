using System.ComponentModel.DataAnnotations;

namespace BirthdayWebsiteAPI.ViewModels.Guest
{
    public class CreateGuestByUserViewModel
    {
        [Required]
        [MaxLength(100, ErrorMessage = "The name needs 100 characters")]
        public string FullName { get; set; }
        [Required]
        [RegularExpression(@"^\(?\d{2}\)?\s?9\d{4}-\d{4}$", ErrorMessage = "WhatsApp number must be in the format (XX) 9XXXX-XXXX or XX 9XXXX-XXXX")]
        public string WhatsApp { get; set; }
    }
}
