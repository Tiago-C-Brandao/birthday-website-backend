using System.ComponentModel.DataAnnotations;

namespace BirthdayWebsiteAPI.ViewModels.Account
{
    public class RegisterViewModel
    {
        [Required]
        [MaxLength(20, ErrorMessage = "The name needs 20 characters")]
        public string UserName { get; set; }

        [Required]
        [MaxLength(100, ErrorMessage = "The name needs 100 characters")]
        public string FullName { get; set; }
        
        [Required]
        [RegularExpression(@"^\(?\d{2}\)?\s?9\d{4}-\d{4}$", ErrorMessage = "WhatsApp number must be in the format (XX) 9XXXX-XXXX or XX 9XXXX-XXXX")]
        public string WhatsApp { get; set; }
        
        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        
        [Required]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "Passwords don't match")]
        public string ConfirmPassword { get; set; }
    }
}
