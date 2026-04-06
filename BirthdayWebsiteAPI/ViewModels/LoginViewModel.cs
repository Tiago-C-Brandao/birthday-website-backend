using System.ComponentModel.DataAnnotations;

namespace BirthdayWebsiteAPI.ViewModels
{
    public class LoginViewModel
    {
        [Required]
        [MaxLength(20, ErrorMessage = "The name needs 20 characters")]
        public string UserNameOrWhatsapp { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}
